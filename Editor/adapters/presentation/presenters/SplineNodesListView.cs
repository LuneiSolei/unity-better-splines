using LuneiSolei.BetterSplines.Adapters;
using UnityEngine.UIElements;

namespace LuneiSolei.BetterSplines.Editor.Adapters.Presenters
{
    public class SplineNodesListView : IPresenter
    {
        private readonly ListView _field;
        private readonly BetterSplineComponent _component;

        public SplineNodesListView(ListView field, BetterSplineComponent component)
        {
            _field = field;
            _component = component;
        }
        
        public void Initialize()
        {
            // Override default behaviors
            _field.onAdd = OnAdd;
            _field.onRemove = OnRemove;
        }

        public void Dispose() { }

        private void OnAdd(BaseListView element)
        {
            _component.AddSplineNode();
        }

        private void OnRemove(BaseListView element)
        {
            SplineNode node = _component.GetLastNode();
            _component.DestroySplineNode(node);
        }
    }
}