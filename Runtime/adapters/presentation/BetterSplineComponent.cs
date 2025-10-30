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
        
        /// <summary>
        /// Updates all spline nodes along the assigned spline.
        /// </summary>
        private void UpdateNodes()
        {
            Shared.Logger.Debug(
                message: "Updating nodes",
                data: new Dictionary<string, object>
                {
                    {"splineIndex", splineIndex},
                    {"splineContainer", splineContainer}
                },
                context: this);
            
            // Spawn each node in the spline nodes list
            for (int i = 0; i < splineNodes.Count; i++)
            {
                // Spawn the spline node
                SplineNode node = splineNodes[i];
                float ratio = EvaluateRatio(i);
                node.ValidateGameObject(defaultGameObject);
                node.UpdatePosition(AssignedSpline, ratio, splineContainer.transform);
                node.Spawn();
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
            
            // Destroy all relevant spawned objects
            foreach (SplineNode node in splineNodes)
            {
                node.prefab = null;
                if (node.IsSpawned) node.Despawn();
            }
            splineNodes.Clear();
        }
    }
}
