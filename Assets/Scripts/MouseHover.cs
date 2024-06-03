using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Used to detect if the mouse is hovering over text to change its color
public class MouseHover : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshPro>().color = Color.black;
    }

    void OnMouseEnter()
    {
        GetComponent<TextMeshPro>().color = Color.red;
    }
    
    void OnMouseExit()
    {
        GetComponent<TextMeshPro>().color = Color.black;
    }
}
