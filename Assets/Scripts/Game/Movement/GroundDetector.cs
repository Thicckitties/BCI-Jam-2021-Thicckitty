using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{

    public enum GroundDetectionType
    {
        [InspectorName("Use Raycast Offsets")]
        TYPE_USE_RAYCAST_OFFSETS,
        [InspectorName("Use Box Collider")]
        TYPE_USE_BOX_COLLIDER
    }
    
    [System.Serializable]
    public struct GroundDetectionData
    {
        [System.Serializable]
        public struct RaycastData
        {      
            [SerializeField]
            public Vector3 raycastPositionOffset;
            [SerializeField]
            public float maxDistance;
        }
        
        [SerializeField]
        private GroundDetectionType groundDetectionType;
        [SerializeField]
        private LayerMask raycastLayerMask;
        
        [SerializeField]
        public BoxCollider boxCollider;
        [SerializeField, Min(0.0f)]
        public float boxRaycastDistance;
        [SerializeField]
        public float raycastYOffset;

        [SerializeField]
        public RaycastData defaultRaycastData;
        [SerializeField]
        public List<RaycastData> raycastOffsets;

        public LayerMask RaycastLayerMask => raycastLayerMask;
        public GroundDetectionType GroundDetectionType => groundDetectionType;
    }

    internal class GroundDetectorUtils
    {
        public static bool IsOnGround(Vector3 pos, float maxDistance, LayerMask mask,
            Transform assocTransform)
        {
            RaycastHit[] outputRaycast =
                Physics.RaycastAll(pos,
                    Vector3.down, maxDistance, 
                    mask);

            if (outputRaycast.Length <= 0)
            {
                return false;
            }

            if (outputRaycast.Length == 1)
            {
                return !IsAssociatedWithTransform(outputRaycast[0].transform, assocTransform);
            }

            for (int i = 0; i < outputRaycast.Length; i++)
            {
                if (!IsAssociatedWithTransform(outputRaycast[0].transform, assocTransform))
                {
                    return true;
                }
            }
            return false;
        }
        
        private static bool IsAssociatedWithTransform(Transform a, Transform b)
        {
            if (b == null
                || a == null)
            {
                return false;
            }
            return a.IsChildOf(b)
                   || b.IsChildOf(a);
        }
    }
    
    public interface IGroundDetectionComponent
    {
        Transform Transform
        {
            get;
        }

        GroundDetectionData GroundDetectionData
        {
            get;
        }

        GroundDetectionController GroundDetector
        {
            get;
        }
    }

    public class GroundDetectionController
    {
        private readonly IGroundDetectionComponent _groundDetection;

        public GroundDetectionController(IGroundDetectionComponent component)
        {
            _groundDetection = component;
        }

        public void OnDrawGizmos()
        {
            switch (_groundDetection.GroundDetectionData.GroundDetectionType)
            {
                case GroundDetectionType.TYPE_USE_BOX_COLLIDER:
                {
                    if (_groundDetection.GroundDetectionData.boxCollider)
                    {
                        Gizmos.color = Color.blue;
                        BoxCollider collider = _groundDetection.GroundDetectionData.boxCollider;
                        var bounds = collider.bounds;
                        Vector3 min = bounds.min + Vector3.up * _groundDetection.GroundDetectionData.raycastYOffset;
                        Vector3 max = bounds.max;
                        
                        Gizmos.DrawRay(
                            min,
                            Vector3.down * _groundDetection.GroundDetectionData.boxRaycastDistance);
                        Gizmos.DrawRay(
                            new Vector3(min.x, min.y, max.z),
                            Vector3.down * _groundDetection.GroundDetectionData.boxRaycastDistance);
                        Gizmos.DrawRay(
                            new Vector3(max.x, min.y, max.z),
                            Vector3.down * _groundDetection.GroundDetectionData.boxRaycastDistance);
                        Gizmos.DrawRay(
                            new Vector3(max.x, min.y, min.z),
                            Vector3.down * _groundDetection.GroundDetectionData.boxRaycastDistance);
                    }
                    break;
                }
                case GroundDetectionType.TYPE_USE_RAYCAST_OFFSETS:
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(
                        _groundDetection.Transform.position 
                        + _groundDetection.GroundDetectionData.defaultRaycastData.raycastPositionOffset,
                        Vector3.down * _groundDetection.GroundDetectionData.defaultRaycastData.maxDistance);
            
                    for (int i = 0; i < _groundDetection.GroundDetectionData.raycastOffsets.Count; i++)
                    {
                        GroundDetectionData.RaycastData offset 
                            = _groundDetection.GroundDetectionData.raycastOffsets[i];
                        Gizmos.DrawRay(
                            _groundDetection.Transform.position + offset.raycastPositionOffset,
                            Vector3.down * offset.maxDistance);
                    }
                    break;
                }
            }
        }

        public bool IsOnGround()
        {
            switch (_groundDetection.GroundDetectionData.GroundDetectionType)
            {
                case GroundDetectionType.TYPE_USE_BOX_COLLIDER:
                {
                    BoxCollider collider = _groundDetection.GroundDetectionData.boxCollider;
                    var bounds = collider.bounds;
                    Vector3 min = bounds.min + Vector3.up * _groundDetection.GroundDetectionData.raycastYOffset;
                    Vector3 minXmaxZ = new Vector3(min.x, min.y, bounds.max.z);
                    Vector3 maxXminZ = new Vector3(bounds.max.x, min.y, min.z);
                    Vector3 maxXmaxZ = new Vector3(bounds.max.x, min.y, bounds.max.z);
                    return IsOnGround(min, _groundDetection.GroundDetectionData.boxRaycastDistance)
                           || IsOnGround(minXmaxZ, _groundDetection.GroundDetectionData.boxRaycastDistance)
                           || IsOnGround(maxXmaxZ, _groundDetection.GroundDetectionData.boxRaycastDistance)
                           || IsOnGround(maxXminZ, _groundDetection.GroundDetectionData.boxRaycastDistance);
                }
                case GroundDetectionType.TYPE_USE_RAYCAST_OFFSETS:
                {
                    bool onGround = IsOnGround(_groundDetection.GroundDetectionData.defaultRaycastData);
                    if (onGround)
                    {
                        return true;
                    }
            
                    foreach(var raycastData in _groundDetection.GroundDetectionData.raycastOffsets)
                    {
                        if (IsOnGround(raycastData))
                        {
                            return true;
                        }
                    }
                    break;
                }
            }
            return false;
        }

        private bool IsOnGround(in GroundDetectionData.RaycastData raycastData)
        {
            return IsOnGround(_groundDetection.Transform.position
                              + raycastData.raycastPositionOffset, raycastData.maxDistance);
        }

        private bool IsOnGround(Vector3 pos, float maxDistance)
        {
            return GroundDetectorUtils.IsOnGround(pos,
                maxDistance, _groundDetection.GroundDetectionData.RaycastLayerMask,
                _groundDetection.Transform);
        }
    }
}
