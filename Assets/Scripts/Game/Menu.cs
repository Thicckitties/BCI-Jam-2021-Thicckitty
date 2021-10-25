using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Thicckitty
{
    public class Menu : MonoBehaviour
    {
        public void EnterGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void Credits()
        {
            SceneManager.LoadScene("Credits");

        }

    }
}
