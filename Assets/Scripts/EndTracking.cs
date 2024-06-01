using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Makes endcreen locked onto player position, at all times 
/// </summary>
public class EndTracking : MonoBehaviour
{
    /// <summary>
    /// Target the endscreen is locked onto
    /// </summary>
    public Transform target;
    /// <summary>
    /// How offset the endscreen should be
    /// </summary>
    [SerializeField]
    public Vector3 offset;

    [SerializeField]
    Game game;

    [SerializeField]
    GameObject win;

    [SerializeField]
    GameObject lose;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // get target position, apply offset, and set as endscreen position
        transform.position = target.position + offset;
        transform.eulerAngles = new Vector3(0,0,0);
    }

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