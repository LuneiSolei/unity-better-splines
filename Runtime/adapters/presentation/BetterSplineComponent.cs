using UnityEngine;
using UnityEngine.Splines;

namespace LuneiSolei.BetterSplines.Adapters
{
    [AddComponentMenu("Splines/Better Spline")]
    public class BetterSplineComponent : MonoBehaviour
    {
        [SerializeField]
        private SplineContainer splineComponent;
        public SplineContainer SplineComponent
        {
            get => splineComponent;
            set => splineComponent = value;
        }
    
        private void Reset()
        {
            // Set defaults
            SplineComponent = GetComponent<SplineContainer>();
        
        }
    }
}
