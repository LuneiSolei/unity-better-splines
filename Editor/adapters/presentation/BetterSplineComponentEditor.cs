using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using LuneiSolei.BetterSplines.Adapters;
using LuneiSolei.BetterSplines.Editor.Adapters.Presenters;
using LuneiSolei.BetterSplines.Editor.Shared;
using UnityEditor.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters
{
    [CustomEditor(typeof(BetterSplineComponent))]
    public class BetterSplineComponentEditor : UnityEditor.Editor
    {
        private BetterSplineComponent _component;
        private VisualElement _root;
        private readonly List<IPresenter> _presenters = new();
        
        // Constants
        private static class FieldProperties
        {
            public const string SplineIndex = "splineIndex";
        }

        private static class FieldNames
        {
            public const string SplineDropdown = "SplineDropdown";
            public const string NodeList = "NodeList";
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of the Inspector UI.
            _root = new VisualElement();
            LoadUIAssets();
            SetUpPresenters();
            
            // Bind the serialized object
            _root.Bind(serializedObject);
            
            return _root;
        }

        private void LoadUIAssets()
        {
            // Load UXML
            VisualTreeAsset uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(InspectorConfig.InspectorUxml);
            if (uxmlAsset == null) return; // This should never happen!
            VisualElement uxmlContent = uxmlAsset.CloneTree();
            _root.Add(uxmlContent);
            
            // Load USS
            StyleSheet ussAsset = AssetDatabase.LoadAssetAtPath<StyleSheet>(InspectorConfig.InspectorUss);
            if (ussAsset == null) return; // This should never happen either!
            _root.styleSheets.Add(ussAsset);
        }

        private void SetUpPresenters()
        {
            // Get the target BetterSplineComponent
            _component = (BetterSplineComponent)target;
            
            // Set up SplineIndexDropdown
            DropdownField splineIndexDropdown = _root.Q<DropdownField>(FieldNames.SplineDropdown);

            if (splineIndexDropdown != null)
            {
                SerializedProperty property = serializedObject.FindProperty(FieldProperties.SplineIndex);
                IPresenter presenter = new SplineIndexDropdown(splineIndexDropdown, _component, property);
                presenter.Initialize();
                _presenters.Add(presenter);
            }
            
            // Set up SplineNodesListview
            ListView nodesList = _root.Q<ListView>(FieldNames.NodeList);
            if (nodesList != null)
            {
                IPresenter presenter = new SplineNodesList(nodesList, _component);
                presenter.Initialize();
                _presenters.Add(presenter);
            }
        }

        private void OnDestroy()
        {
            foreach (IPresenter presenter in _presenters) presenter.Dispose();
        }
    }
}