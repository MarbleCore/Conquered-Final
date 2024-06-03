using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Allows player to go back to main menu when they're in the settings menu
public class SettingsMenuBack : MonoBehaviour
{
    public bool isBack;

    void OnMouseUp()
    {
        if(isBack)
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
