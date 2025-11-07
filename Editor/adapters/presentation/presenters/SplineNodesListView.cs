using LuneiSolei.BetterSplines.Adapters;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    internal class SplineNodesListView : IPresenter
    {
        private readonly ListView _field;
        private readonly BetterSplineComponent _component;
        private readonly SerializedObject _serializedObject;
        private SerializedProperty _listProperty;

        private static class PropertyNames
        {
            public const string SplineNodes = "splineNodes";
            public const string Name = "name";
        }
        
        private static class FieldNames
        {
            public const string Name = "nameField";
        }

        public SplineNodesListView(ListView field, BetterSplineComponent component, SerializedObject serializedObject)
        {
            _field = field;
            _component = component;
            _serializedObject = serializedObject;
        }
        
        public void Initialize()
        {
            _listProperty = _serializedObject.FindProperty(PropertyNames.SplineNodes);
            VisualTreeAsset splineNodeTemplate =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Shared.EditorConsts.SplineNodeUxmlPath);
            
            // Override default behaviors
            _field.onAdd = OnAdd;
            _field.onRemove = OnRemove;
            _field.itemTemplate = splineNodeTemplate;
            _field.bindItem = OnBindItem;
            _field.unbindItem = OnUnbindItem;
        }

        public void Dispose() { }

        private void OnAdd(BaseListView element)
        {
            _component.AddSplineNode();
        }

        private void OnRemove(BaseListView element)
        {
            SplineNode node = _component.GetLastSplineNode();
            _component.DestroySplineNode(node);
        }

        private void OnBindItem(VisualElement element, int index)
        {
            // Get the "name" TextField
            TextField nameField = element.Q<TextField>(FieldNames.Name);
            
            if (nameField == null) return;
            
            // Bind property
            SerializedProperty itemProperty = _listProperty.GetArrayElementAtIndex(index);
            SerializedProperty nameProperty = itemProperty.FindPropertyRelative(PropertyNames.Name);
            nameField.BindProperty(nameProperty);
            
            // Create presenter
            IPresenter presenter = new NameTextField(nameField, _component, index);
            presenter.Initialize();
            element.userData = presenter;
        }

        private void OnUnbindItem(VisualElement element, int index)
        {
            IPresenter presenter = (IPresenter)element.userData;
            
            if (presenter == null) return;

            presenter.Dispose();
            element.userData = null;
        }
    }
}