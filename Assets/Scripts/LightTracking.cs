using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LightingTracking
{
    /// <summary>
    /// Makes light locked onto player position, at all times 
    /// </summary>
    public class LightingTracking : MonoBehaviour
    {
        /// <summary>
        /// Target the light is locked onto
        /// </summary>
        public Transform target;
        /// <summary>
        /// How offset the light should be
        /// </summary>
        [SerializeField]
        public Vector3 offset;

        int isRGBOn;

        void Awake(){
            isRGBOn = PlayerPrefs.GetInt("RGB", 0);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // get target position, apply offset, and set as camera position
            transform.position = target.position + offset;

            if(isRGBOn == 1)
            {
                float red = Mathf.PingPong(Time.time, 1);
                float green = Mathf.PingPong(Time.time * 0.7f, 1);
                float blue = Mathf.PingPong(Time.time * 1.5f, 1);

                GetComponent<Light>().color = new Color(red, green, blue);
            }
        }
    }
}