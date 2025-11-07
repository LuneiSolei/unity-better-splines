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
            // Load assets
            VisualTreeAsset uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorConsts.ComponentEditorUxmlPath);
            StyleSheet ussAsset = AssetDatabase.LoadAssetAtPath<StyleSheet>(EditorConsts.ComponentEditorUssPath);
            
            // Ensure we've loaded our assets correctly
            if (uxmlAsset == null || ussAsset == null) return;
            
            // Add assets to the root
            VisualElement uxmlContent = uxmlAsset.CloneTree();
            _root.Add(uxmlContent);
            _root.styleSheets.Add(ussAsset);
        }

        private void SetUpPresenters()
        {
            // Get the target BetterSplineComponent
            BetterSplineComponent component = (BetterSplineComponent)target;
            
            // Set up SplineIndexDropdown
            DropdownField splineIndexDropdown = _root.Q<DropdownField>(FieldNames.SplineDropdown);

            if (splineIndexDropdown != null)
            {
                SerializedProperty property = serializedObject.FindProperty(FieldProperties.SplineIndex);
                IPresenter presenter = new SplineIndexDropdown(splineIndexDropdown, component, property);
                presenter.Initialize();
                _presenters.Add(presenter);
            }
            
            // Set up SplineNodesListview
            ListView nodesList = _root.Q<ListView>(FieldNames.NodeList);
            if (nodesList != null)
            {
                IPresenter presenter = new SplineNodesListView(nodesList, component, serializedObject);
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