using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHS.UI
{
    public class CHS_MainMenuManager : MonoBehaviour
    {
        private void Start()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        public void LoadScene(int BuildIndex)
        {
            SceneManager.LoadScene(BuildIndex);
        }
    }
}
