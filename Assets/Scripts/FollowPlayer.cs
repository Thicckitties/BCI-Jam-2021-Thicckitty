using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Thicckitty
{
    public class FollowPlayer : MonoBehaviour
    {
        // Needs a dolly path
        // dead distances from my scene: 12, -9
        // Thanks and Enjoy! -Dex

        private CinemachineTrackedDolly dolly;
        
        [SerializeField] private SODA.Vector3Reference soda;
        [SerializeField] private float deadDistance;
        [SerializeField] private float backDeadDistance;
        private float playerStartPos;
        private float lastPos;

        // Start is called before the first frame update
        void Start()
        {
            dolly = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
            playerStartPos = lastPos = soda.Value.z;
        }


        // Update is called once per frame
        void LateUpdate()
        {
            if ((soda.Value.z - playerStartPos) - dolly.m_PathPosition * dolly.m_Path.PathLength > deadDistance || (soda.Value.z - playerStartPos) - dolly.m_PathPosition * dolly.m_Path.PathLength < backDeadDistance)
            {
                dolly.m_PathPosition += (soda.Value.z - lastPos) / dolly.m_Path.PathLength;
            }
            lastPos = soda.Value.z;
                
        }

    }
}
