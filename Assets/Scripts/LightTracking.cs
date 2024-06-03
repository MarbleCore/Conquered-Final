using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes light locked onto player position, at all times 
public class LightingTracking : MonoBehaviour
{
    //Target the light is locked onto
    public Transform target;

    //How offset the light should be
    [SerializeField]
    public Vector3 offset;

    //Int that's used to determine if RGB setting is enabled
    int isRGBOn;

    //Whenever awaken, gets RGB setting, defaulting to 0 meaning it isn't enabled
    void Awake(){
        isRGBOn = PlayerPrefs.GetInt("RGB", 0);
    }

    //Update is called once per frame
    void LateUpdate()
    {
        //Get target position, apply offset, and set as light position
        transform.position = target.position + offset;

        //Enables RGB setting if it was turned on
        if(isRGBOn == 1)
        {
            float red = Mathf.PingPong(Time.time, 1);
            float green = Mathf.PingPong(Time.time * 0.7f, 1);
            float blue = Mathf.PingPong(Time.time * 1.5f, 1);

            GetComponent<Light>().color = new Color(red, green, blue);
        }
    }
}