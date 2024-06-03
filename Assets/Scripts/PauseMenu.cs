using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Allows player to quit to main menu when they pause the game
public class PauseMenu : MonoBehaviour
{
    public bool isQuitToMain;

    void OnMouseUp()
    {
        if(isQuitToMain)
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}