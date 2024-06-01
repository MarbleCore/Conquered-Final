using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// Makes character sprite locked onto player position, at all times 
    /// </summary>
    public class CharacterTracking : MonoBehaviour
    {
        /// <summary>
        /// Target the character sprite is locked onto
        /// </summary>
        public Transform target;
        /// <summary>
        /// How offset the character sprite should be
        /// </summary>
        [SerializeField]
        public Vector3 offset;

        // Update is called once per frame
        void LateUpdate()
        {
            // get target position, apply offset, and set as character sprite position
            transform.position = target.position + offset;
            transform.eulerAngles = new Vector3(0,180,0);
        }
    }
