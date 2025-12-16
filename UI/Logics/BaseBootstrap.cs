using Code.UI.Logics;
using UnityEngine;

namespace Code.UI.Logics
{
    public abstract class BaseBootstrap<TModel, TView, TPresenter> : MonoBehaviour
        where TModel : BaseModel, new()
        where TView : BaseView
        where TPresenter : BasePresenter<TModel, TView>
    {
        [SerializeField] protected TView view;
        protected TModel model;
        protected TPresenter presenter;

        protected virtual void Awake() {
            model = CreateModel();
            presenter = CreatePresenter(model, view);
        }

        protected virtual TModel CreateModel() => new TModel();
        protected abstract TPresenter CreatePresenter(TModel model, TView view);

        protected virtual void OnDestroy() {
            presenter?.Dispose();
        }
    }
}