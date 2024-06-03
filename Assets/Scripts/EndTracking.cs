using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes endcreen locked onto player position, at all times 
public class EndTracking : MonoBehaviour
{
    //Target the endscreen is locked onto
    public Transform target;

    //How offset the endscreen should be
    [SerializeField]
    public Vector3 offset;

    //References game gameobject
    [SerializeField]
    Game game;

    //References win text gameobject
    [SerializeField]
    GameObject win;

    //References lose text gameobject
    [SerializeField]
    GameObject lose;

    //Initially turns gameobject off
    void Start()
    {
        gameObject.SetActive(false);
    }

    //Update is called once per frame
    void LateUpdate()
    {
        //Get target position, apply offset, and set as endscreen position
        transform.position = target.position + offset;
        transform.eulerAngles = new Vector3(0,0,0);
    }

    //Runs when game ends, and display corresponding win or lose text based on which text gameobject to disable
    public void GameEnd()
    {
        gameObject.SetActive(true);
        if (game.isAlive == true)
        {
            lose.SetActive(false);
        }
        else
        {
            win.SetActive(false);
        }
    }
}