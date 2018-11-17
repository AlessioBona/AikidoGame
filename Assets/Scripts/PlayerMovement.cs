using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody rb;
    public float movementSpeed;
    public float jumpForce;
    bool canJump = true;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal1") != 0.0f || Input.GetAxis("Vertical1") != 0.0f) { 
        rb.AddForce(new Vector3(-Input.GetAxis("Horizontal1")*movementSpeed, 0, Input.GetAxis("Vertical1")*movementSpeed));
        }
        if (Input.GetButton("Jump1") && rb.transform.position.y < 0.7f)
        {
            Debug.Log("jumping");
            canJump = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if(other.tag == "FloorCollider")
    //    {
    //        canJump = true;
    //    }
    //}
    
}
