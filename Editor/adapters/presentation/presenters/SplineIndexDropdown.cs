using System.Collections.Generic;
using LuneiSolei.BetterSplines.Adapters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    public class SplineIndexDropdown : IPresenter
    {
        private readonly DropdownField _field;
        private readonly BetterSplineComponent _component;
        private readonly SerializedProperty _property;

        public SplineIndexDropdown(
            DropdownField field,
            BetterSplineComponent component,
            SerializedProperty property)
        {
            _field = field;
            _component = component;
            _property = property;
        }
        
        public void Initialize()
        {
            if (_component.SplineContainer == null) return;
            
            RefreshChoices();
            
            // Register callbacks
            SplineContainer.SplineAdded += OnSplineCountChanged;
            _field.RegisterValueChangedCallback(OnValueChanged);
            Undo.undoRedoPerformed += RefreshChoices;
        }

        public void Dispose()
        {
            // Unregister callbacks
            SplineContainer.SplineAdded -= OnSplineCountChanged;
            _field.UnregisterValueChangedCallback(OnValueChanged);
            Undo.undoRedoPerformed -= RefreshChoices;
        }

        private void RefreshChoices()
        {
            if (_component.SplineContainer == null) return;

            _property.serializedObject.Update();
            _field.choices = GetSplineList();

            _field.SetValueWithoutNotify(GetSplineNameAtIndex(_property.intValue));
        }
        
        private List<string> GetSplineList()
        {
            List<string> list = new();
            for (int i = 0; i < _component.SplineContainer.Splines.Count; i++) list.Add($"Spline {i}");
            
            return list;
        }

        private string GetSplineNameAtIndex(int index)
        {
            if (index < 0 || index >= _component.SplineContainer.Splines.Count)
                return _field.choices.Count > 0 ? _field.choices[0] : "";

            return _field.choices[index];
        }

        private void OnSplineCountChanged(SplineContainer container, int index)
        {
            if (container == _component.SplineContainer) RefreshChoices();
        }

        private void OnValueChanged(ChangeEvent<string> evt)
        {
            _property.intValue = _field.choices.IndexOf(evt.newValue);
            _property.serializedObject.ApplyModifiedProperties();
        }
    }
}