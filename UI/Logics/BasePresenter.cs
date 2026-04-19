using Code.UI.Logics;
using UnityEngine;

namespace Code.UI.Logics
{
    public abstract class BasePresenter <TModel, TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        protected TModel Model { get; }
        protected TView View { get; }

        protected BasePresenter(TModel model, TView view) {
            Model = model;
            View = view;
            OnInit();
            
            Model.OnChanged += OnModelChanged;
        }

        protected virtual void OnInit() { }

        public virtual void Dispose()
        {
            Model.OnChanged -= OnModelChanged;
        }

        protected abstract void OnModelChanged();
    }
}