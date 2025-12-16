using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Code.Units.UnitStat
{
    public class Stat
    {
        private Dictionary<object, float> _modifyValueByKey = new Dictionary<object, float>();
        private float _modifiedValue;
        
        public delegate void ValueChangeHandler(Stat stat, float currentValue, float prevValue);
        public event ValueChangeHandler OnValueChanged;
        
        private UnitStatType _statTypeType;
        public UnitStatType StatTypeType => _statTypeType;
        
        private readonly float _baseValue = 1;

        public float Value => _baseValue + _modifiedValue;

        private bool _isMoveSpeedModifier;
        public bool IsMoveSpeedModifier => _isMoveSpeedModifier;

        public bool CanApplyStat { get; set; } = true; 

        public Stat(UnitStatType statType)
        {
            _statTypeType = statType;
        }

        public void AddModifier(object key, float value, bool increaseStack = false)
        {
            if(increaseStack == false && _modifyValueByKey.ContainsKey(key) || !CanApplyStat) return;
            
            if(_statTypeType == UnitStatType.MoveSpeed && _isMoveSpeedModifier) return;
            
            if (_modifyValueByKey.ContainsKey(key)) // 이미 있으면 있던거에 추가
            {
                _modifyValueByKey[key] += value;
            }
            else // 새거면 Add
            {
                _modifyValueByKey.Add(key, value);
            }

            float prevValue = Value;
            _modifiedValue += value;
            TryInvokeValueChangeEvent(Value, prevValue);
            
            if(_statTypeType == UnitStatType.MoveSpeed)
                _isMoveSpeedModifier = true;
        }

        public void RemoveModifier(object key)
        {
            if (_modifyValueByKey.TryGetValue(key, out float value))
            {
                if(_statTypeType == UnitStatType.MoveSpeed && !_isMoveSpeedModifier || !CanApplyStat) return;
                
                _isMoveSpeedModifier = false;
                
                float prevValue = Value;
                _modifiedValue -= value;
                _modifyValueByKey.Remove(key);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        public void ClearModifier()
        {
            float prev = Value;
            _modifyValueByKey.Clear();
            _modifiedValue = 0;
            TryInvokeValueChangeEvent(Value, prev);
        }
        
        private void TryInvokeValueChangeEvent(float value, float prevValue)
        {
            //이전값과 바뀐값이 일치하지 않는다면 변경 이벤트를 콜해주는 함수다.
            if (Mathf.Approximately(value, prevValue) == false)
            {
                OnValueChanged?.Invoke(this, value, prevValue);
            }
        }

        #region Debug region
        internal Dictionary<object, float> ModifyPair => _modifyValueByKey;
        #endregion
    }
}