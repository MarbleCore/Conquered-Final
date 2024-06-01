using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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