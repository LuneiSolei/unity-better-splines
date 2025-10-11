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
    
        private void Reset()
        {
            // Set defaults
            SplineContainer = GetComponent<SplineContainer>();
        }
    }
}
