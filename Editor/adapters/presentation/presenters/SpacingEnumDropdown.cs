using System;
using System.Linq;
using LuneiSolei.BetterSplines.Adapters;
using LuneiSolei.BetterSplines.Shared;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    public class SpacingEnumDropdown : IPresenter
    {
        private readonly EnumField _field;
        private readonly BetterSplineComponent _component;
        private readonly SerializedProperty _property;
        
        public SpacingEnumDropdown(EnumField field, BetterSplineComponent component, SerializedProperty property)
        {
            _field = field;
            _component = component;
            _property = property;
        }
        
        public void Initialize()
        {
            // _field.RegisterValueChangedCallback(OnValueChanged);
        }

        public void Dispose()
        {
            // _field.UnregisterValueChangedCallback(OnValueChanged);
        }
        
        // private void OnValueChanged(ChangeEvent<string> evt)
        // {
        //     Undo.RecordObject(_component, "Change Default Node Spacing");
        //     
        //     _component.DefaultNodeSpacing = (NodeSpacing)evt.newValue;
        //     EditorUtility.SetDirty(_component);
        // }
    }
}