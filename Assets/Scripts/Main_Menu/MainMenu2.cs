using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu2 : MonoBehaviour
{

    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("Single_Player");
    }

    public void LoadCoopModeGame()
    {
        SceneManager.LoadScene("Coop_Mode");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

#if UNITY_EDITOR
            // If we are in the Editor...
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
