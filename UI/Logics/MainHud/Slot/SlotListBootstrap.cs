using Code.UI.Logics.MainHud.Slot.View;
using Code.UI.Logics.MainHud.Slot.Model;
using Code.UI.Logics.MainHud.Slot.Presenter;
using EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UI.Logics.MainHud.Slot
{
    public class SlotListBootstrap : BaseBootstrap<SlotListModel, SlotListView, SlotListPresenter>
    {
        [SerializeField] private GameEventChannelSO slotEventChannel;
        protected override SlotListPresenter CreatePresenter(SlotListModel model, SlotListView view)
        {
            presenter = new SlotListPresenter(model, view, slotEventChannel);
            return presenter;
        }
    }
}