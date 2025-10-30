namespace LuneiSolei.BetterSplines.Editor.Shared
{
    internal static class InspectorConfig
    {
        private const string PackageRoot = "Packages/com.luneisolei.bettersplines";
        private const string EditorRoot = PackageRoot + "/Editor";
        private const string InspectorRoot = EditorRoot + "/adapters/presentation";
        private const string InspectorName = "BetterSplineComponentEditor";
        
        public const string InspectorUxml = InspectorRoot + "/" + InspectorName + ".uxml";
        public const string InspectorUss = InspectorRoot + "/" + InspectorName + ".uss";
    }
}