using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles switching scenes when corresponding buttons are clicked
public class MainMenu : MonoBehaviour
{
    //For the start game button
    public bool isStart;

    //For the settings button
    public bool isSettings;

    //For the quit button
    public bool isQuit;

    //Waits for mouse to be released before loading corresponding scene
    void OnMouseUp()
    {
        //Loads gamescene
        if(isStart)
        {
            SceneManager.LoadScene("GameScene");
        }
        
        //Loads settingscene
        if(isSettings)
        {
            SceneManager.LoadScene("SettingsScene");
        }

        //Quits the application
        if(isQuit)
        {
            Application.Quit();
        }
    }
}
