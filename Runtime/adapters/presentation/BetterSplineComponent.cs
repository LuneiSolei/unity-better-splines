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
        
        [SerializeField]
        private int splineIndex;
        public int SplineIndex
        {
            get => splineIndex;
            set => splineIndex = value;
        }
        
        [SerializeField]
        private GameObject defaultGameObject;
        public GameObject DefaultGameObject
        {
            get => defaultGameObject;
            set => defaultGameObject = value;
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
