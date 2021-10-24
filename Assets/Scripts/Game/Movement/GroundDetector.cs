using UnityEngine;

namespace Thicckitty
{

    [System.Serializable]
    public struct GroundDetectionData
    {
        [SerializeField]
        private LayerMask raycastLayerMask;
        [SerializeField]
        private Vector3 raycastOffset;
        [SerializeField, Min(0.01f)]
        private float raycastDistance;

        public Vector3 RaycastOffset => raycastOffset;
        public float RaycastDistance => raycastDistance;
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
            Vector3 pos = _groundDetection.Transform.position + _groundDetection.GroundDetectionData.RaycastOffset;
            Gizmos.DrawLine(pos,
                pos + Vector3.down * _groundDetection.GroundDetectionData.RaycastDistance);
        }

        public bool IsOnGround()
        {
            RaycastHit[] outputRaycast =
                Physics.RaycastAll(_groundDetection.Transform.position 
                     + _groundDetection.GroundDetectionData.RaycastOffset, 
                    Vector3.down, _groundDetection.GroundDetectionData.RaycastDistance, 
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
