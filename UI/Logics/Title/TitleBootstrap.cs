using Code.UI.Logics.Title.Model;
using Code.UI.Logics.Title.Presenter;
using Code.UI.Logics.Title.View;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.Title
{
    public class TitleBootstrap : BaseBootstrap<TitleModel, TitleView, TitlePresenter>
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        protected override TitlePresenter CreatePresenter(TitleModel model, TitleView view)
        {
            return new TitlePresenter(model, view, uiEventChannel);
        }
    }
}