using System;
using UnityEngine;

namespace Code.UI.Logics.MainHud.Slot.Model
{
    public class EnergyModel : BaseModel
    {
        public int CurrentEnergy { get; private set; }
        public int MaxEnergy { get; private set; }
        public Action OnConsumed;
        
        public void SetEnergy(int current, int max)
        {
            CurrentEnergy = current;
            MaxEnergy = max;
        }
        
        public void AddEnergy(int amount)
        {
            CurrentEnergy = Mathf.Clamp(CurrentEnergy + amount, 0, MaxEnergy);
            NotifyChanged();
        }
        public void ConsumeEnergy(int amount)
        {
            CurrentEnergy = Mathf.Clamp(CurrentEnergy - amount, 0, MaxEnergy);
            OnConsumed?.Invoke();
            NotifyChanged();
        }
    }
}