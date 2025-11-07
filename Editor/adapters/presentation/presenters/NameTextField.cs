using LuneiSolei.BetterSplines.Adapters;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    internal class NameTextField : IPresenter
    {
        private readonly TextField _field;
        private readonly BetterSplineComponent _component;
        private readonly int _index;

        internal NameTextField(TextField field, BetterSplineComponent component, int index)
        {
            _field = field;
            _component = component;
            _index = index;
        }
        
        public void Initialize()
        {
            // Register callbacks
            _field.RegisterValueChangedCallback(OnValueChanged);
        }

        public void Dispose()
        {
            // Unregister callbacks
            _field.UnregisterValueChangedCallback(OnValueChanged);
        }

        private void OnValueChanged(ChangeEvent<string> evt)
        {
            SplineNode node = _component.GetSplineNode(_index);
            if (node == null) return;
            
            node.Name = evt.newValue;
        }
    }
}