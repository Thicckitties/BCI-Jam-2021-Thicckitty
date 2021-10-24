using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Thicckitty
{
    public class FollowPlayer : MonoBehaviour
    {
        [Header("SODA")]
        [SerializeField] private SODA.Vector3Reference playerPosition;
        
        [Header("Dead Distance")]
        [SerializeField] private float deadDistance;
        [SerializeField] private float backDeadDistance;
        
        private float _playerStartPos;
        private float _lastPos;
        
        private CinemachineTrackedDolly _dollyComponent;

        private void Start()
        {
            _dollyComponent = GetComponent<CinemachineVirtualCamera>()
                ?.GetCinemachineComponent<CinemachineTrackedDolly>();
            _playerStartPos = _lastPos = playerPosition.Value.z;
        }

        private void LateUpdate()
        {
            if (_dollyComponent)
            {
                float difference = playerPosition.Value.z - _playerStartPos
                                                          - (_dollyComponent.m_PathPosition * _dollyComponent.m_Path.PathLength);
                if (difference > deadDistance 
                    || difference < backDeadDistance)
                {
                    _dollyComponent.m_PathPosition 
                        += (playerPosition.Value.z - _lastPos) / _dollyComponent.m_Path.PathLength;
                }
            }
            _lastPos = playerPosition.Value.z;
        }
    }
}
