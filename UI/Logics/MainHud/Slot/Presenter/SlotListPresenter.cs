using Code.UI.Logics.MainHud.Slot.Model;
using Code.UI.Logics.MainHud.Slot.View;
using Code.Units.UnitDatas;
using EventSystem;

namespace Code.UI.Logics.MainHud.Slot.Presenter
{
    public class SlotListPresenter : BasePresenter<SlotListModel, SlotListView>
    {
        private GameEventChannelSO _slotEventChannel;
        private UnitDataSO _nextUnitData;

        public SlotListPresenter(SlotListModel model, SlotListView view,
            GameEventChannelSO slotEventChannel)
            : base(model, view)
        {
            _slotEventChannel = slotEventChannel;

            model.SetSlotCount(view.Container.childCount);
            RenderAll();

            slotEventChannel.AddListener<SlotAddEvent>(HandleSlotAddEvent);
        }

        public override void Dispose()
        {
            base.Dispose();
            _slotEventChannel.RemoveListener<SlotAddEvent>(HandleSlotAddEvent);
        }


        #region Slot rotation logic
        private void HandleSlotAddEvent(SlotAddEvent obj)
        {
            // if (Model.ItemSetUpCount < Model.IndexList.Length)
            // {
            //     View.SetUpSlot(Model.IndexList[Model.ItemSetUpCount], obj.unitData.icon, obj.unitData.classType);
            //     Model.ItemSetUpCount++;
            // }
            // else
            // {
            View.SetNextItem(obj.unitData);
            Model.PopToBack();
            // }
        }

        public void PopSlotItem() => Model.PopToBack();

        #endregion

        protected override void OnModelChanged() => RenderAll();

        private void RenderAll()
        {
            View.UpdateView(Model.IndexList);
        }
    }
}