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
        private readonly BetterSplineComponent _component;

        public SplineNodesList(ListView field, BetterSplineComponent component)
        {
            _field = field;
            _component = component;
        }
        
        public void Initialize()
        {
            // Register callbacks
            _field.itemsAdded += OnValueChanged;
            _field.itemsRemoved += OnValueChanged;
        }

        public void Dispose()
        {
            // Unregister callbacks
            _field.itemsAdded -= OnValueChanged;
            _field.itemsRemoved -= OnValueChanged;
        }

        private void OnValueChanged(IEnumerable<int> indices)
        {
            _component.UpdateNodes();
        }
    }
}