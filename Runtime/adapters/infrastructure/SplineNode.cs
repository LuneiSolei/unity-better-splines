using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

namespace LuneiSolei.BetterSplines.Adapters
{
    [Serializable]
    public class SplineNode
    {
        [SerializeField] internal string name;
        [SerializeField] private Vector3 worldPosition;
        [SerializeField] private Transform parentTransform;
        [SerializeField] internal GameObject prefab;
        [SerializeField] internal GameObject spawnedInstance;
        internal bool IsSpawned => spawnedInstance != null;

        /// <summary>
        /// Ensures the spline node uses its assigned game object or the default game object as defined by the
        /// BetterSplineComponent.
        /// </summary>
        /// <param name="defaultGameObject"></param>
        internal void ValidateGameObject(GameObject defaultGameObject)
        {
            if (prefab == null) prefab = defaultGameObject;
        }

        internal void Destroy()
        {
            Shared.Logger.Debug("Destroying Node", new Dictionary<string, object> {{"Node", name}});
#if UNITY_EDITOR
            UnityEngine.Object.DestroyImmediate(spawnedInstance);      
#else
            UnityEngine.Object.Destroy(spawnedInstance);
#endif
        }
        
        /// <summary>
        /// Executes spawning based on environment (build target).
        /// </summary>
        internal GameObject Spawn()
        {
            Shared.Logger.Debug(
                message: "Spawning node",
                data: new Dictionary<string, object>
                {
                    {"prefab", prefab},
                    {"worldPosition", worldPosition},
                    {"parentTransform", parentTransform},
                    {"spawnedInstance", spawnedInstance}
                });
            
            // Don't respawn unnecessarily
            if (IsSpawned) return null;

            if (prefab == null)
            {
                Shared.Logger.NodePrefabIsNull(parentTransform.gameObject);

                return null;
            }
            
#if UNITY_EDITOR
                EditorSpawn();
#else
                RuntimeSpawn();
#endif

            return spawnedInstance;
        }

        /// <summary>
        /// Updates the world position for the spline node.
        /// </summary>
        /// <param name="spline">The spline on which to evaluate the world position.</param>
        /// <param name="ratio">The ratio along the spline.</param>
        /// <param name="transform">The Transform of the spline container that contains the spline.</param>
        /// <returns>The new world position.</returns>
        internal Vector3 UpdatePosition(Spline spline, float ratio, Transform transform)
        {
            float3 localPosition = spline.EvaluatePosition(ratio);
            worldPosition = transform.TransformPoint(localPosition);
            parentTransform = transform;
            
            // If an instance exists, update its position
            if (IsSpawned) spawnedInstance.transform.position = worldPosition;
            
            Shared.Logger.Debug(
                message: "Updated spline node position",
                data: new Dictionary<string, object>
                {
                    {"spline", spline},
                    {"knotCount", spline.Count},
                    {"localPosition", localPosition},
                    {"worldPosition", worldPosition},
                    {"ratio", ratio}
                });

            return worldPosition;
        }
        
        /// <summary>
        /// Spawns a clone of the game object assigned to the spline node.
        /// </summary>
        private void RuntimeSpawn()
        {
            spawnedInstance = UnityEngine.Object.Instantiate(
                original: prefab,
                position: worldPosition,
                rotation: Quaternion.identity,
                parent: parentTransform);
            spawnedInstance.hideFlags = HideFlags.DontSave;
            spawnedInstance.name = name ?? "";
        }

        /// <summary>
        /// Spawns a prefab instance of the game object assigned to the spline node.
        /// </summary>
        private void EditorSpawn()
        {
            Shared.Logger.Debug(
                message:"Spawning spline node in editor",
                data: new Dictionary<string, object>
                {
                    {"prefab", prefab},
                    {"worldPosition", worldPosition},
                    {"parentTransform", parentTransform},
                });
            
            spawnedInstance = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
            spawnedInstance.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
            spawnedInstance.transform.SetParent(parentTransform);
            spawnedInstance.name = name ?? "";
        }
    }
}