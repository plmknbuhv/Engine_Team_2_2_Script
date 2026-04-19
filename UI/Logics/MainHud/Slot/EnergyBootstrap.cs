using Code.UI.Logics.MainHud.Slot.Model;
using Code.UI.Logics.MainHud.Slot.Presenter;
using Code.UI.Logics.MainHud.Slot.View;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.MainHud.Slot
{
    public class EnergyBootstrap : BaseBootstrap<EnergyModel, EnergyView, EnergyPresenter>
    {
        [SerializeField] private int maxEnergy = 20;
        [SerializeField] private GameEventChannelSO costEventChannel;
        protected override EnergyPresenter CreatePresenter(EnergyModel model, EnergyView view)
        {
            return new EnergyPresenter(model, view, maxEnergy, costEventChannel);
        }
    }
}