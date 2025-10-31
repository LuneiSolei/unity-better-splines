using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LuneiSolei.BetterSplines.Shared;
using UnityEngine;
using UnityEngine.Splines;

[assembly: InternalsVisibleTo("LuneiSolei.BetterSplines.Editor")]
namespace LuneiSolei.BetterSplines.Adapters
{
    [ExecuteAlways, AddComponentMenu("Splines/Better Spline")]
    public class BetterSplineComponent : MonoBehaviour
    {
        // Spline
        [SerializeField] internal SplineContainer splineContainer;
        [SerializeField] private int splineIndex;
        
        // Defaults
        [SerializeField] private GameObject defaultGameObject;
        [SerializeField] private NodeSpacing defaultNodeSpacing = NodeSpacing.EvenSpacing;
        
        // Misc.
        [SerializeField] private List<SplineNode> splineNodes = new();
        private readonly HashSet<SplineNode> _trackedSplineNodes = new();

        /// <summary>
        /// The spline that the BetterSplineComponent will perform operations on.
        /// </summary>
        private Spline AssignedSpline
        {
            get
            {
                if (splineContainer == null ||
                    splineIndex < 0 ||
                    splineIndex >= splineContainer.Splines.Count) return null;

                return splineContainer.Splines[splineIndex];
            }
        }

        internal void AddSplineNode()
        {
            SplineNode newNode = new();
            splineNodes.Add(newNode);
            UpdateNodes();
        }

        internal SplineNode GetLastNode()
        {
            return splineNodes[^1];
        }

        /// <summary>
        /// Remove a spline node from the component's list. The associated GameObject, if any, will automatically be
        /// destroyed.
        /// </summary>
        /// <param name="node">The node to destroy.</param>
        internal void DestroySplineNode(SplineNode node)
        {
            DestroySplineNodeInternal(node);
            _trackedSplineNodes.Remove(node);
        }

        /// <summary>
        /// Internal method used for common actions between Reset() and DestroySplineNode().
        /// </summary>
        /// <param name="node"></param>
        private void DestroySplineNodeInternal(SplineNode node)
        {
            // Check for desynchronized tracking
            bool isInList = splineNodes.Contains(node);
            bool isInHashSet = _trackedSplineNodes.Contains(node);
            
            if (!isInList && !isInHashSet) return; // Node is not managed by this component instance

            // Destroy instance
            node.Destroy();
            splineNodes.Remove(node);
        }
        
        /// <summary>
        /// Updates all spline nodes along the assigned spline.
        /// </summary>
        internal void UpdateNodes()
        {
            for (int i = 0; i < splineNodes.Count; i++)
            {
                // Spawn the spline node
                SplineNode node = splineNodes[i];
                node.name = $"Spline Node {i}";
                float ratio = EvaluateRatio(i);
                node.ValidateGameObject(defaultGameObject);
                node.UpdatePosition(AssignedSpline, ratio, splineContainer.transform);
                node.Spawn();
                
                _trackedSplineNodes.Add(node);
            }
        }

        /// <summary>
        /// Evaluates a ratio along the assigned spline. The ratio is dependent upon the selected node spacing method.
        /// </summary>
        /// <param name="index">The index of the spline node to evaluate.</param>
        /// <returns>A ratio representative of the relevant spline node's position along the assigned spline.</returns>
        private float EvaluateRatio(int index)
        {
            switch (defaultNodeSpacing)
            {
                case NodeSpacing.EvenSpacing:
                    return (float)index / (splineNodes.Count - 1);
            }

            return 0.0f;
        }
        
        /// <summary>
        /// Called when the assigned spline has been modified.
        /// </summary>
        /// <param name="changedSpline">The spline that was modified.</param>
        /// <param name="knotIndex">The index of the knot that was changed.</param>
        /// <param name="modification">An object containing information about the modification(s).</param>
        private void OnSplineChanged(Spline changedSpline, int knotIndex, SplineModification modification)
        {
            // Ensure the changed spline matches the one from our cached index
            if (!splineContainer.Splines[splineIndex].Equals(changedSpline)) return;

            UpdateNodes();
        }
        
        private void OnEnable()
        {
            UpdateNodes();
            Spline.Changed += OnSplineChanged;
        }
        
        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }
        
        private void Reset()
        {
            // Set defaults
            splineContainer = GetComponent<SplineContainer>();
            splineIndex = splineContainer.Splines.Count > 0 ? 0 : -1;
            defaultGameObject = null;
            defaultNodeSpacing = NodeSpacing.EvenSpacing;

            foreach (SplineNode node in _trackedSplineNodes)
            {
                DestroySplineNodeInternal(node);;
            }

            _trackedSplineNodes.Clear();
        }
    }
}
