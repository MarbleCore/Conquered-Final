using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool isStart;

    public bool isSettings;
    public bool isQuit;

    void OnMouseUp()
    {
        if(isStart)
        {
            SceneManager.LoadScene("GameScene");
        }
        if(isSettings)
        {
            SceneManager.LoadScene("SettingsScene");
        }
        if(isQuit)
        {
            Application.Quit();
        }
    }
}
