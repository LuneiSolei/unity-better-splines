using System.Collections.Generic;
using LuneiSolei.BetterSplines.Adapters;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    public class SplineNodesList : IPresenter
    {
        private readonly ListView _field;
        private readonly SerializedProperty _property;
        private readonly BetterSplineComponent _component;

        public SplineNodesList(ListView field, SerializedProperty property, BetterSplineComponent component)
        {
            _field = field;
            _property = property;
            _component = component;
        }
        
        public void Initialize()
        {
            // Register callbacks
            _field.itemsAdded += OnValueChanged;
        }

        public void Dispose()
        {
            // Unregister callbacks
            _field.itemsAdded -= OnValueChanged;
        }

        private void OnValueChanged(IEnumerable<int> indices)
        {
            _component.UpdateNodes();
        }
    }
}