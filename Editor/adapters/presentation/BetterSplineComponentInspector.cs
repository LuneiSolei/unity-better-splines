using UnityEditor;
using UnityEngine.UIElements;
using LuneiSolei.BetterSplines.Adapters;
using LuneiSolei.BetterSplines.Editor.Shared;
using UnityEditor.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters
{
    [CustomEditor(typeof(BetterSplineComponent))]
    public class BetterSplineComponentInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of the Inspector UI.
            VisualElement inspector = new();
            
            // Load UXML
            VisualTreeAsset uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(InspectorConfig.InspectorUxml);
            if (uxmlAsset == null) return inspector; // This should never happen!
            VisualElement uxmlContent = uxmlAsset.CloneTree();
            inspector.Add(uxmlContent);
            
            // Load USS
            StyleSheet ussAsset = AssetDatabase.LoadAssetAtPath<StyleSheet>(InspectorConfig.InspectorUss);
            if (ussAsset == null) return inspector; // This should never happen either!
            inspector.styleSheets.Add(ussAsset);
            
            // Bind the serialized object
            inspector.Bind(serializedObject);
            
            return inspector;
        }
    }
}
