using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using LuneiSolei.BetterSplines.Adapters;
using LuneiSolei.BetterSplines.Editor.Shared;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Splines;

namespace LuneiSolei.BetterSplines.Editor.Adapters
{
    [CustomEditor(typeof(BetterSplineComponent))]
    public class BetterSplineComponentInspector : UnityEditor.Editor
    {
        private static class FieldNames
        {
            public const string SplineDropdown = "SplineDropdown";
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of the Inspector UI.
            VisualElement inspector = new();
            
            // Load and set up UXML/USS
            LoadUIAssets(inspector);
            
            // Set up spline dropdown
            SetUpSplineDropdown(inspector);
            
            // Bind the serialized object
            inspector.Bind(serializedObject);
            
            return inspector;
        }

        private static void LoadUIAssets(VisualElement inspector)
        {
            // Load UXML
            VisualTreeAsset uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(InspectorConfig.InspectorUxml);
            if (uxmlAsset == null) return; // This should never happen!
            VisualElement uxmlContent = uxmlAsset.CloneTree();
            inspector.Add(uxmlContent);
            
            // Load USS
            StyleSheet ussAsset = AssetDatabase.LoadAssetAtPath<StyleSheet>(InspectorConfig.InspectorUss);
            if (ussAsset == null) return; // This should never happen either!
            inspector.styleSheets.Add(ussAsset);
        }

        private void SetUpSplineDropdown(VisualElement inspector)
        {
            BetterSplineComponent component = (BetterSplineComponent)target;
            DropdownField field = inspector.Q<DropdownField>(FieldNames.SplineDropdown);
            if (component.SplineContainer == null) return;

            field.choices = GetSplineList();
            field.index = component.SplineIndex;

            return;

            List<string> GetSplineList()
            {
                List<string> list = new();
                for (int i = 0; i < component.SplineContainer.Splines.Count; i++)
                {
                    Debug.Log("Adding Spline");
                    list.Add($"Spline {i}");
                }
                
                return list;
            }
        }
    }
}
