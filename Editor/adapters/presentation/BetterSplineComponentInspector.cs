using UnityEditor;
using UnityEngine.UIElements;
using LuneiSolei.BetterSplines.Adapters;
using LuneiSolei.BetterSplines.Editor.Shared;

namespace LuneiSolei.BetterSplines.Editor.Adapters
{
    [CustomEditor(typeof(BetterSplineComponent))]
    public class BetterSplineComponentInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of the Inspector UI.
            VisualElement inspector = new();
            VisualTreeAsset uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(InspectorConfig.InspectorUxml);
            StyleSheet ussAsset = AssetDatabase.LoadAssetAtPath<StyleSheet>(InspectorConfig.InspectorUss);
            
            // Load UXML file
            if (uxmlAsset == null) return inspector;
            VisualElement uxmlContent = uxmlAsset.CloneTree();
            inspector.Add(uxmlContent);
            inspector.styleSheets.Add(ussAsset);

            return inspector;
        }
    }
}
