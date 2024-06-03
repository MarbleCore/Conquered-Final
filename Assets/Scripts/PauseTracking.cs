using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes pausescreen locked onto player position, at all times 
public class PauseTracking : MonoBehaviour
{
    //Target the pausescreen is locked onto
    public Transform target;

    //How offset the pausescreen should be
    [SerializeField]
    public Vector3 offset;

    //Sets gameobject to false when game starts
    void Start()
    {
        gameObject.SetActive(false);
    }

    //Update is called once per frame
    void LateUpdate()
    {
        //Get target position, apply offset, and set as pausescreen position
        transform.position = target.position + offset;
        transform.eulerAngles = new Vector3(0,0,0);
    }

    //When the game is paused, this gameobject is set to active
    public void Pause()
    {
        gameObject.SetActive(true);
    }

    //When the game is unpaused, this gameobject is set to inactive
    public void Unpause()
    {
        gameObject.SetActive(false);
    }
}