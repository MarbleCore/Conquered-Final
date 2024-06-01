using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// Makes camera locked onto player position, at all times 
    /// </summary>
    public class CameraTracking : MonoBehaviour
    {
        /// <summary>
        /// Target the camera is locked onto
        /// </summary>
        public Transform target;
        /// <summary>
        /// How offset the camera should be
        /// </summary>
        [SerializeField]
        public Vector3 offset;

        // Update is called once per frame
        void LateUpdate()
        {
            // get target position, apply offset, and set as camera position
            transform.position = target.position + offset;
            transform.eulerAngles = new Vector3(90,0,0);
        }

        public void Off()
        {
            gameObject.SetActive(false);
        }

        public void On()
        {
            gameObject.SetActive(true);
        }
    }
