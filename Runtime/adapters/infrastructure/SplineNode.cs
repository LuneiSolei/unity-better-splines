using UnityEngine;

namespace LuneiSolei.BetterSplines.Adapters
{
    [System.Serializable]
    public class SplineNode
    {
        [SerializeField]
        private GameObject gameObject;
        public GameObject GameObject
        {
            get => gameObject;
            set => gameObject = value;
        }

        [SerializeField]
        private Vector3 worldPosition;
        public Vector3 WorldPosition
        {
            get => worldPosition;
            set => worldPosition = value;
        }
    }
}