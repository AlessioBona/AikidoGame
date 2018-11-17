﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canJumpOn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "jumpChecker")
        {
            other.GetComponentInParent<PlayerMovement>().state.canJump = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "jumpChecker")
        {
            other.GetComponentInParent<PlayerMovement>().state.canJump = false;
        }
    }
}
