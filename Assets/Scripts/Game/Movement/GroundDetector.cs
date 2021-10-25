using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{

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
        private LayerMask raycastLayerMask;
        [SerializeField]
        public RaycastData defaultRaycastData;
        [SerializeField]
        public List<RaycastData> raycastOffsets;

        public LayerMask RaycastLayerMask => raycastLayerMask;
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
        }

        public bool IsOnGround()
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
            return false;
        }

        private bool IsOnGround(in GroundDetectionData.RaycastData raycastData)
        {
            return IsOnGround(raycastData.raycastPositionOffset, raycastData.maxDistance);
        }

        private bool IsOnGround(Vector3 offset, float maxDistance)
        {
            RaycastHit[] outputRaycast =
                Physics.RaycastAll(_groundDetection.Transform.position 
                                   + offset,
                    Vector3.down, maxDistance, 
                    _groundDetection.GroundDetectionData.RaycastLayerMask);

            if (outputRaycast.Length <= 0)
            {
                return false;
            }

            if (outputRaycast.Length == 1)
            {
                return !IsAssociatedWithTransform(outputRaycast[0].transform);
            }

            for (int i = 0; i < outputRaycast.Length; i++)
            {
                if (!IsAssociatedWithTransform(outputRaycast[0].transform))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAssociatedWithTransform(Transform transform)
        {
            if (transform == null
                || _groundDetection.Transform == null)
            {
                return false;
            }
            return _groundDetection.Transform.IsChildOf(transform)
                   || transform.IsChildOf(_groundDetection.Transform);
        }
    }
}
