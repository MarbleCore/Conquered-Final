using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Toggle that turns on and off the RGB lighting setting
public class SettingsMenuRGB : MonoBehaviour
{
    //int used to determine whether RGB lighting is enabled or disabled
    int isRGB;

    //References toggle gameobject
    [SerializeField]
    Toggle toggle;

    //Sets isRGB value when awoken, 0 by default, and sets toggle state based on it
    void Awake()
    {
        isRGB = PlayerPrefs.GetInt("RGB", 0);
        GameObject.Find("Toggle").GetComponent<Toggle>().isOn = isRGB != 0;
    }

    //Switches isRGB value based on toggle, affecting RGB lighting setting (and used in lightingtracking)
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
