using System;
using Code.UI.Visual;
using Code.Units.UnitDatas;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Logics.MainHud.Slot.View
{
    public class SlotListView : BaseView
    {
        [SerializeField] private Transform container;
        [SerializeField] private UIElement titleElement;
        public Transform Container => container;
        private SlotItemController[] _boots;

        private const string ConsumeValueKey = "consume";

        private UnitDataSO _nextUnitData;

        private void Awake()
        {
            _boots = container.GetComponentsInChildren<SlotItemController>(true);
        }

        public async void UpdateView(int[] indexList)
        {
            try
            {
                titleElement.PlayFeedback(ConsumeValueKey);

                var length = indexList.Length;
                var prevIndex = indexList[length - 1];
                var prevFront = _boots[prevIndex];

                await prevFront.Hide();

                for (int i = 0; i < length; i++)
                {
                    var boot = _boots[indexList[i]];
                    boot.transform.SetSiblingIndex(i);
                }

                var newFront = _boots[indexList[0]];
                newFront.SetHighlight(true);
                if (_nextUnitData != null)
                    prevFront.SetUpSlot(_nextUnitData);
                prevFront.Show().Forget();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void SetNextItem(UnitDataSO data) => _nextUnitData = data;
    }
}