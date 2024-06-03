using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //Makes camera locked onto player position, at all times 
    public class CameraTracking : MonoBehaviour
    {
        //Target the camera is locked onto, player is assigned here
        public Transform target;

        //How offset the camera should be
        [SerializeField]
        public Vector3 offset;

        //Update is called once per frame
        void LateUpdate()
        {
            //Get target position, apply offset, and set as camera position
            transform.position = target.position + offset;
            transform.eulerAngles = new Vector3(90,0,0);
        }

        //Turns camera gameobject on
        public void On()
        {
            gameObject.SetActive(true);
        }
        
        //Turns camera gameobject off
        public void Off()
        {
            gameObject.SetActive(false);
        }
    }
