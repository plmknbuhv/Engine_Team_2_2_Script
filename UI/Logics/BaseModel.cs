using System;

namespace Code.UI.Logics
{
    public abstract class BaseModel
    {
        public Action OnChanged;
        
        protected void NotifyChanged()
        {
            OnChanged?.Invoke();
        }
    }
}