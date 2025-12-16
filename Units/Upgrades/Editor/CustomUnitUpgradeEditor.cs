using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Units.Upgrades.Editor
{
    [UnityEditor.CustomEditor(typeof(UnitPerkUpgradeSO))]
    public class CustomUnitUpgradeEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset customTreeAsset;

        private UnitPerkUpgradeSO _unitPerkUpgradeSo;
        private Assembly _unitAssembly;
        private VisualElement _root;
        private DropdownField _fieldList;
        private IntegerField _intField;
        private FloatField _floatField;

        public override VisualElement CreateInspectorGUI()
        {
            _unitPerkUpgradeSo = target as UnitPerkUpgradeSO;

            _root = new VisualElement();
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);
            customTreeAsset.CloneTree(_root);
            MakeSkillDropDown();
            
            EnumField upgradeTypeSelector = _root.Q<EnumField>("UpgradeType");
            upgradeTypeSelector.RegisterValueChangedCallback(HandleUpgradeTypeChange);

            _intField = _root.Q<IntegerField>("IntegerValue");
            _floatField = _root.Q<FloatField>("FloatValue");
            _fieldList = _root.Q<DropdownField>("FieldList");
            UpdateFieldList();
            
            return _root;
        }


        private void HandleUpgradeTypeChange(ChangeEvent<Enum> evt)
        {
            UpdateFieldList();

            switch (evt.newValue)
            {
                case UnitPerkUpgradeSO.UpgradeType.Boolean:
                    _intField.style.display = DisplayStyle.None;
                    _floatField.style.display = DisplayStyle.None;
                    _fieldList.style.display = DisplayStyle.Flex;
                    break;
                case UnitPerkUpgradeSO.UpgradeType.Integer:
                    _intField.style.display = DisplayStyle.Flex;
                    _floatField.style.display = DisplayStyle.None;
                    _fieldList.style.display = DisplayStyle.Flex;
                    break;
                case UnitPerkUpgradeSO.UpgradeType.Float:
                    _intField.style.display = DisplayStyle.None;
                    _floatField.style.display = DisplayStyle.Flex;
                    _fieldList.style.display = DisplayStyle.Flex;
                    break;
                case UnitPerkUpgradeSO.UpgradeType.Method:
                    _intField.style.display = DisplayStyle.None;
                    _floatField.style.display = DisplayStyle.None;
                    _fieldList.style.display = DisplayStyle.None;
                    break;
            }
        }

        private void UpdateFieldList()
        {
            _fieldList.choices = _unitPerkUpgradeSo.upgradeType switch
            {
                UnitPerkUpgradeSO.UpgradeType.Boolean => _unitPerkUpgradeSo.boolFields.Select(field => field.Name).ToList(),
                UnitPerkUpgradeSO.UpgradeType.Integer => _unitPerkUpgradeSo.intFields.Select(field => field.Name).ToList(),
                UnitPerkUpgradeSO.UpgradeType.Float => _unitPerkUpgradeSo.floatFields.Select(field => field.Name).ToList(),
                UnitPerkUpgradeSO.UpgradeType.Method => new List<string>(),
                _ => new List<string>()
            };
            
            if (_fieldList.choices.Count > 0 &&
                _fieldList.choices.Contains(_unitPerkUpgradeSo.selectFieldName) == false)
            {
                _unitPerkUpgradeSo.selectFieldName = _fieldList.choices[0];
            }else if (_fieldList.choices.Count == 0)
            {
                _unitPerkUpgradeSo.selectFieldName = string.Empty;
            }
        }
        
        private void MakeSkillDropDown()
        {
            DropdownField typeSelector = _root.Q<DropdownField>("TypeSelector");

            _unitAssembly = Assembly.GetAssembly(typeof(Unit));
            List<Type> derivedTypes = _unitAssembly.GetTypes()
                .Where(type => type.IsClass && type.IsAbstract == false && type.IsSubclassOf(typeof(Unit)))
                .ToList();
            
            derivedTypes.ForEach(type => typeSelector.choices.Add(type.FullName));

            typeSelector.RegisterValueChangedCallback(HandleTypeChange);
            typeSelector.SetValueWithoutNotify(_unitPerkUpgradeSo.targetUnit);
            
            _unitPerkUpgradeSo.targetUnit = typeSelector.value;
            _unitPerkUpgradeSo.GetFieldsFromTargetUnit();
        }

        private void HandleTypeChange(ChangeEvent<string> evt)
        {
            _unitPerkUpgradeSo.targetUnit = evt.newValue;
            _unitPerkUpgradeSo.GetFieldsFromTargetUnit();
            UpdateFieldList();
        }
    }
}