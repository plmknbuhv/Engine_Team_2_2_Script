using UnityEngine;

namespace Code.UI.Logics.MainHud.Stat.Model
{
    public class StatModel : BaseModel
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }

        public void SetHealth(int current)
        {
            CurrentHealth = current;
            NotifyChanged();
        }

        public void SetMaxHealth(int max)
        {
            MaxHealth = max;
            NotifyChanged();
        }

        public int CurrentEnergy { get; set; }
        public int MaxEnergy { get; set; }

        public void SetEnergy(int current)
        {
            CurrentEnergy = current;
            NotifyChanged();
        }

        public void SetMaxEnergy(int max)
        {
            MaxEnergy = max;
            NotifyChanged();
        }

        public float CurrentExp { get; set; }
        public float MaxExp { get; set; }
        public int CurrentLevel { get; set; }

        public void SetExp(float current)
        {
            CurrentExp = current;
            NotifyChanged();
        }

        public void SetMaxExp(float max)
        {
            Debug.Log("SetMaxExp: " + max);
            MaxExp = max;
            NotifyChanged();
        }

        public void SetLevel(int level)
        {
            CurrentLevel = level;
            NotifyChanged();
        }
    }
}