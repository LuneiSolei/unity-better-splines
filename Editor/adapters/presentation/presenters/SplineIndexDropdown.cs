using System.Collections.Generic;
using LuneiSolei.BetterSplines.Adapters;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    public class SplineIndexDropdown : IPresenter
    {
        private readonly DropdownField _field;
        private readonly BetterSplineComponent _component;

        public SplineIndexDropdown(DropdownField field, BetterSplineComponent component)
        {
            _field = field;
            _component = component;
        }
        
        public void Initialize()
        {
            if (_component.SplineContainer == null) return;
            
            RefreshChoices();
            SplineContainer.SplineAdded += OnSplineCountChanged;
        }

        public void Dispose()
        {
            SplineContainer.SplineAdded -= OnSplineCountChanged;
        }

        private void RefreshChoices()
        {
            if (_component.SplineContainer == null) return;

            string currentValue = _field.value;
            _field.choices = GetSplineList();
            
            // Restore selection or clamp to valid range
            int newIndex = _field.choices.IndexOf(currentValue);
            _field.index = newIndex >= 0 ? newIndex : Mathf.Clamp(
                _component.SplineIndex,
                0,
                _field.choices.Count - 1);
        }
        
        private List<string> GetSplineList()
        {
            List<string> list = new();
            for (int i = 0; i < _component.SplineContainer.Splines.Count; i++) list.Add($"Spline {i}");
            
            return list;
        }

        private void OnSplineCountChanged(SplineContainer container, int index)
        {
            if (container == _component.SplineContainer) RefreshChoices();
        }
    }
}