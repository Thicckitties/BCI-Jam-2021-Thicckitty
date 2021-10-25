using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Thicckitty
{
    public class Restart : MonoBehaviour
    {
        [SerializeField]
        private string restartedScene;
        
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(restartedScene);
                Destroy(gameObject);
                return;
            }
        }
    }
}
