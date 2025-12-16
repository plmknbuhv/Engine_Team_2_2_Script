using Code.UI.Logics.MainHud.Wave.Model;
using Code.UI.Logics.MainHud.Wave.Presenter;
using Code.UI.Logics.MainHud.Wave.View;
using Enemies;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.UI.Logics.MainHud.Wave
{
    public class WaveBootStrap : BaseBootstrap<WaveModel, WaveView, WavePresenter>
    {
        [SerializeField] private GameEventChannelSO waveChannel;
        [SerializeField] private GameEventChannelSO enemyChannel;
        [Inject] private EnemyManager _enemyManager;
        protected override WavePresenter CreatePresenter(WaveModel model, WaveView view)
        {
            return new WavePresenter(model, view, waveChannel, enemyChannel, _enemyManager);
        }
    }
}