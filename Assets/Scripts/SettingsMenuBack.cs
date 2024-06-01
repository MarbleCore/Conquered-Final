using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
