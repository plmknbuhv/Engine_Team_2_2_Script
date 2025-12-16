using Code.UI.Logics.MainHud.Stat.Model;
using Code.UI.Logics.MainHud.Stat.Presenter;
using Code.UI.Logics.MainHud.Stat.View;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.MainHud.Stat
{
    public class StatBootstrap : BaseBootstrap<StatModel, StatView, StatPresenter>
    {
        [SerializeField] private GameEventChannelSO costChannel;
        [SerializeField] private GameEventChannelSO levelChannel;
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private int maxEnergy = 15;
        [SerializeField] private int requiredExp = 20;
        protected override StatPresenter CreatePresenter(StatModel model, StatView view)
        {
            return new StatPresenter(model, view, maxEnergy, requiredExp, costChannel, levelChannel, cannonChannel);
        }
    }
}