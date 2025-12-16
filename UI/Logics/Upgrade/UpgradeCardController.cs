using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.UI.Visual;
using Code.Upgrades;
using Code.Upgrades.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.Logics.Upgrade
{
    public class UpgradeCardController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UIElement cardElement;
        [SerializeField] private UIElement gradeElement;
        [SerializeField] private Image raycastImage;

        [Header("References")] [SerializeField]
        private TextMeshProUGUI typeText;

        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI gradeText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Transform showTrm; 

        [Header("Text References")] [SerializeField]
        private UpgradeTypeToString[] upgradeTypeToStrings;

        private Dictionary<UpgradeType, string> _typeToStringDict;

        private int _index;

        private const string _hideKey = "hide";
        private const string _selectKey = "select";

        public Action<int> OnClickEvent;
        public Action<int, bool> OnHoverEvent;

        private string _selectedType;
        private string _selectedGrade = "default";
        
        private UpgradeDataSO _currentUpgradeData;

        public void Init(int idx)
        {
            _index = idx;
            _typeToStringDict = new Dictionary<UpgradeType, string>();
            foreach (var typeString in upgradeTypeToStrings)
                _typeToStringDict[typeString.type] = typeString.typeString;
        }

        public async UniTask SetCardInfo(UpgradeDataSO upgradeData)
        {
            _currentUpgradeData = upgradeData;
            Debug.Log("Setting card info for: " + upgradeData.displayName + " of type " + upgradeData.upgradeType);

            var newTypeText = _typeToStringDict.ContainsKey(upgradeData.upgradeType)
                ? _typeToStringDict[upgradeData.upgradeType]
                : "Unknown";
            Debug.Log("Setting type text: " + newTypeText);
            typeText.text = newTypeText;
            Debug.Log("Setting name text: " + upgradeData.displayName);
            nameText.text = upgradeData.displayName;

            Debug.Log("Setting card image for: " + upgradeData.displayName);
            cardImage.sprite = upgradeData.icon;

            Debug.Log("Clearing grade text");
            gradeText.text = $"";
            Debug.Log("Setting description text: " + upgradeData.description);
            descriptionText.text = upgradeData.description;

            if (upgradeData.upgradeType == UpgradeType.AddUnit)
            {
                var unitAddData = upgradeData as UnitUnlockDataSO;
                if (unitAddData == null)
                {
                    Debug.LogError("UpgradeDataSO is not of type UnitUnlockDataSO");
                    return;
                }

                Debug.Log( "Setting unit add data for: " + upgradeData.displayName);
                var targetUnitData = unitAddData.unitData;
                Debug.Log("Setting unit name text: " + targetUnitData.unitName);
                nameText.text = targetUnitData.unitName;
                Debug.Log("Setting grade text: " + targetUnitData.classType);
                gradeText.text = $"[{targetUnitData.classType}]";
                Debug.Log("Setting description text: " + targetUnitData.description);
                descriptionText.text = targetUnitData.description;

                var newGrade = targetUnitData.classType.ToString().ToLower();
                if (_selectedGrade != newGrade)
                {
                    await gradeElement.RemoveState(_selectedGrade);
                    _selectedGrade = targetUnitData.classType.ToString().ToLower();
                    await gradeElement.AddState(_selectedGrade, 20);
                }
            }

            Debug.Log($"Done setting card info for: {upgradeData.displayName}");
            var newType = upgradeData.upgradeType.ToString().ToLower();
            if (_selectedType != newType)
            {
                if (!string.IsNullOrEmpty(_selectedType))
                    await cardElement.RemoveState(_selectedType);
                _selectedType = newType;
                await cardElement.AddState(_selectedType, 10);
            }
        }

        public async UniTask ShowCard(Vector3 position, float duration, Ease ease)
        {
            raycastImage.raycastTarget = false;
            transform.position = position;
            cardElement.AddState(_hideKey, 10).Forget();
            cardElement.RemoveState(_selectKey).Forget();
            transform.DOMove(showTrm.position, duration).SetEase(ease).SetUpdate(true);
            await UniTask.WhenAll(UniTask.WaitForSeconds(duration, true), cardElement.RemoveState(_hideKey));
            raycastImage.raycastTarget = true;
        }

        public async UniTask HideCard(Vector3 position, float duration, Ease ease)
        {
            raycastImage.raycastTarget = false;
            transform.DOMove(position, duration).SetEase(ease).SetUpdate(true);
            await UniTask.WhenAll(UniTask.WaitForSeconds(duration, true), cardElement.AddState(_hideKey, 10));
        }

        public void OnPointerClick(PointerEventData eventData) => OnClickEvent?.Invoke(_index);
        public void OnPointerEnter(PointerEventData eventData) => OnHoverEvent?.Invoke(_index, true);
        public void OnPointerExit(PointerEventData eventData) => OnHoverEvent?.Invoke(_index, false);
    }

    [Serializable]
    public struct UpgradeTypeToString
    {
        public UpgradeType type;
        public string typeString;
    }
}