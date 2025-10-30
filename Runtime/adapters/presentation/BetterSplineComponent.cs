using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LuneiSolei.BetterSplines.Shared;
using UnityEngine;
using UnityEngine.Splines;

namespace LuneiSolei.BetterSplines.Adapters
{
    [ExecuteAlways, AddComponentMenu("Splines/Better Spline")]
    public class BetterSplineComponent : MonoBehaviour
    {
        [SerializeField]
        private SplineContainer splineContainer;
        public SplineContainer SplineContainer
        {
            get => splineContainer;
            set => splineContainer = value;
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
