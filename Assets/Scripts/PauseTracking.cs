using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Makes pausescreen locked onto player position, at all times 
/// </summary>
public class PauseTracking : MonoBehaviour
{
    /// <summary>
    /// Target the pausescreen is locked onto
    /// </summary>
    public Transform target;
    /// <summary>
    /// How offset the pausescreen should be
    /// </summary>
    [SerializeField]
    public Vector3 offset;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // get target position, apply offset, and set as pausescreen position
        transform.position = target.position + offset;
        transform.eulerAngles = new Vector3(0,0,0);
    }

    public void Pause()
    {
        gameObject.SetActive(true);
    }

    public void Unpause()
    {
        gameObject.SetActive(false);
    }
}