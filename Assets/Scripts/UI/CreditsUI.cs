using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thicckitty
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField]
        private string sceneToTransitionTo;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(sceneToTransitionTo);
            }
        }
    }
}
