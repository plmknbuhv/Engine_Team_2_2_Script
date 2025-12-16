using System.Collections.Generic;
using UnityEngine;

namespace Code.Units.UnitStat
{
    [CreateAssetMenu(fileName = "UnitStatMultiply", menuName = "SO/Unit/Stat/UnitStatMultiply", order = 0)]    
    public class UnitStatMultiplySO : ScriptableObject
    {
        private Dictionary<object, float> _modifyValueByKey = new Dictionary<object, float>();
        private float _modifiedValue;
        
        public delegate void ValueChangeHandler(UnitStatMultiplySO unitStatMultiply, float currentValue, float prevValue);
        public event ValueChangeHandler OnValueChanged;
        
        public UnitTeamType teamType;
        public UnitStatType statTypeType;
        public float baseValue = 1;

        private void OnEnable()
        {
            ClearModifier();
        }

        public float Value => baseValue + _modifiedValue;
        
        public void AddModifier(object key, float value)
        {
            if (_modifyValueByKey.ContainsKey(key)) return;

            float prevValue = Value;
            _modifiedValue += value;
            _modifyValueByKey.Add(key, value);
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyValueByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                _modifiedValue -= value;
                _modifyValueByKey.Remove(key);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        public void ClearModifier()
        {
            float prevValue = Value;
            _modifyValueByKey.Clear();
            _modifiedValue = 0;
            TryInvokeValueChangeEvent(Value, prevValue);
        }
        
        private void TryInvokeValueChangeEvent(float value, float prevValue)
        {
            //이전값과 바뀐값이 일치하지 않는다면 변경 이벤트를 콜해주는 함수다.
            if (Mathf.Approximately(value, prevValue) == false)
            {
                OnValueChanged?.Invoke(this, value, prevValue);
            }
        }
    }
}