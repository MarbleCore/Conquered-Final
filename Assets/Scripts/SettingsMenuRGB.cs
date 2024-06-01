using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuRGB : MonoBehaviour
{
    int isRGB;

    [SerializeField]
    Toggle toggle;

    void Awake()
    {
        isRGB = PlayerPrefs.GetInt("RGB", 0);
        GameObject.Find("Toggle").GetComponent<Toggle>().isOn = isRGB != 0;
    }

    public void ToggleRGB()
    {
        if (isRGB == 0 ){
            isRGB = 1;
            PlayerPrefs.SetInt("RGB", 1);
        }
        else{
            isRGB = 0;  
            PlayerPrefs.SetInt("RGB", 0);
        }
    }
}
