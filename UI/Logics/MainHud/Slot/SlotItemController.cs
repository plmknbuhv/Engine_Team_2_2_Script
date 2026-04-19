using Code.UI.Visual;
using Code.Units.UnitDatas;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Code.UI.Logics.MainHud.Slot
{
    public class SlotItemController : BaseView
    {
        [SerializeField] private Image slotItem;
        [SerializeField] private Image icon;
        [SerializeField] private UIElement content;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private DescriptionController descriptionController;

        private const string ActiveKey = "active";
        private const string HideKey = "hide";
        private string _currentThemeKey;

        public void SetHighlight(bool isActive)
        {
            if (!isActive)
            {
                content.RemoveState(ActiveKey).Forget();
                return;
            }

            content.AddState(ActiveKey, 1).Forget();
        }

        public void SetUpSlot(UnitDataSO data)
        {
            descriptionController.SetUpDescription(data);

            icon.sprite = data.icon;
            costText.text = data.requiredCost.ToString();

            var newThemeKey = data.classType.ToString().ToLower();
            content.AddState(newThemeKey).Forget();

            if (!string.IsNullOrEmpty(_currentThemeKey) && _currentThemeKey != newThemeKey)
                content.RemoveState(_currentThemeKey).Forget();

            _currentThemeKey = newThemeKey;
        }

        public override async UniTask Hide()
        {
            SetRaycastTarget(false);
            await UniTask.WhenAll(
                content.AddState(HideKey, 10),
                content.RemoveState(ActiveKey)
            );
        }

        public override async UniTask Show()
        {
            await content.RemoveState(HideKey);
            SetRaycastTarget(true);
        }

        private void SetRaycastTarget(bool value) => slotItem.raycastTarget = value;
    }
}