using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public string playerID;

    string xMovementAxis;
    string yMovementAxis;
    string jumpButton;

    Rigidbody rb;
    public float movementSpeed;
    public float jumpForce;
    bool canJump = true;

    private void Awake()
    {
        xMovementAxis = "Horizontal" + playerID;
        yMovementAxis = "Vertical" + playerID;
        jumpButton = "Jump" + playerID;
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
        if (Input.GetAxis(xMovementAxis) != 0.0f || Input.GetAxis(yMovementAxis) != 0.0f) { 
        rb.AddForce(new Vector3(Input.GetAxis(xMovementAxis)*movementSpeed, 0, Input.GetAxis(yMovementAxis)*movementSpeed));
        }
        if (Input.GetButton(jumpButton) && rb.transform.position.y < 0.7f)
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
