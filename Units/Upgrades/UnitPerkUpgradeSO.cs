using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Units.Upgrades
{
    [CreateAssetMenu(fileName = "Unit upgrade data", menuName = "SO/Unit/Upgrade/Data", order = 0)]
    public class UnitPerkUpgradeSO : ScriptableObject
    {
        public enum UpgradeType
        {
            Boolean, Integer, Float, Method
        }
        
        [HideInInspector] public string targetUnit;
        public List<FieldInfo> boolFields = new List<FieldInfo>();
        public List<FieldInfo> floatFields = new List<FieldInfo>();
        public List<FieldInfo> intFields = new List<FieldInfo>();
        
        [HideInInspector] public UpgradeType upgradeType;
        [HideInInspector] public string selectFieldName;
        [HideInInspector] public int intValue;
        [HideInInspector] public float floatValue;

        private Type _unitType;
        private FieldInfo _selectedField;
        
        private void OnEnable()
        {
            GetFieldsFromTargetUnit();
            SetSelectedField();
        }
        
        public void GetFieldsFromTargetUnit()
        {
            if (string.IsNullOrEmpty(targetUnit))
            {
                Debug.LogWarning($"No target Unit selected! : {this.name}");
                return;
            }
            
            _unitType = Type.GetType(targetUnit);
            if (_unitType == null)
            {
                Debug.LogWarning($"Target Unit not found : {targetUnit}");
                return;
            }
            
            FieldInfo[] fields = _unitType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            boolFields.Clear();
            floatFields.Clear();
            intFields.Clear();
            
            foreach (FieldInfo field in fields)
            {
                if(field.FieldType == typeof(bool)) 
                    boolFields.Add(field);
                else if(field.FieldType == typeof(float))
                    floatFields.Add(field);
                else if(field.FieldType == typeof(int))
                    intFields.Add(field);
            }
        }
        
        public void SetSelectedField()
        {
            if (_unitType == null) return;
            
            _selectedField = _unitType.GetField(selectFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Debug.Assert(_selectedField != null, $"Selected field is null {selectFieldName}");
        }


        public void UpgradeUnit(Unit unit)
        {
            switch (upgradeType)
            {
                case UpgradeType.Boolean:
                    _selectedField.SetValue(unit, true);
                    break;
                case UpgradeType.Integer:
                {
                    int oldValue = (int)_selectedField.GetValue(unit);
                    _selectedField.SetValue(unit, oldValue + intValue);
                    break;
                }
                case UpgradeType.Float:
                {
                    float oldValue = (float)_selectedField.GetValue(unit);
                    _selectedField.SetValue(unit, oldValue + floatValue);
                    break;
                }
                case UpgradeType.Method:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RollbackUpgrade(Unit unit)
        {
            switch (upgradeType)
            {
                case UpgradeType.Boolean:
                    _selectedField.SetValue(unit, false);
                    break;
                case UpgradeType.Integer:
                {
                    int oldValue = (int)_selectedField.GetValue(unit);
                    _selectedField.SetValue(unit, oldValue - intValue);
                    break;
                }
                case UpgradeType.Float:
                {
                    float oldValue = (float)_selectedField.GetValue(unit);
                    _selectedField.SetValue(unit, oldValue - floatValue);
                    break;
                }
                case UpgradeType.Method:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}