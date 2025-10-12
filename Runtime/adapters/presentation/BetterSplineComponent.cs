using UnityEngine;
using UnityEngine.Splines;

namespace LuneiSolei.BetterSplines.Adapters
{
    [AddComponentMenu("Splines/Better Spline")]
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
            SplineContainer = GetComponent<SplineContainer>();
            SplineIndex = -1;
            DefaultGameObject = null;
        }
    }
}
