using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //Makes character sprite locked onto player position, at all times 
    public class CharacterTracking : MonoBehaviour
    {
        //Target the character sprite is locked onto
        public Transform target;

        //How offset the character sprite should be
        [SerializeField]
        public Vector3 offset;

        //Update is called once per frame
        void LateUpdate()
        {
            //Get target position, apply offset, and set as character sprite position
            transform.position = target.position + offset;
            transform.eulerAngles = new Vector3(0,180,0);
        }
    }
