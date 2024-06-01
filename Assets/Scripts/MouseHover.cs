using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
