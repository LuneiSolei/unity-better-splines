namespace LuneiSolei.BetterSplines.Editor.Shared
{
    internal static class EditorConsts
    {
        // Global
        private const string PackageRoot = "Packages/com.luneisolei.bettersplines";
        private const string EditorAssemblyRoot = PackageRoot + "/Editor/";
        private const string EditorsRoot = EditorAssemblyRoot + "adapters/presentation/";
        
        // Component Inspector
        private const string ComponentEditorName = "BetterSplineComponentEditor";
        internal const string ComponentEditorUxmlPath = EditorsRoot + ComponentEditorName + ".uxml";
        internal const string ComponentEditorUssPath = EditorsRoot + ComponentEditorName + ".uss";
        
        // SplineNode Inspector
        private const string SplineNodeEditorName = "SplineNodeEditor";
        internal const string SplineNodeUxmlPath = EditorsRoot + SplineNodeEditorName + ".uxml";
        internal const string SplineNodeUssPath = EditorsRoot + SplineNodeEditorName + ".uss";
    }
}