using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public string playerID;
    public GameObject cylinder;
    public GameObject grabbingPoint;

    string xMovementAxis;
    string yMovementAxis;
    string jumpButton;
    string grabButton;

    public bool grabbing = false;


    Rigidbody rb;
    public float movementSpeed;
    public float jumpForce;
    bool canJump = true;

    public List<GameObject> grabbableObjects;

    private void Awake()
    {
        xMovementAxis = "Horizontal" + playerID;
        yMovementAxis = "Vertical" + playerID;
        jumpButton = "Jump" + playerID;
        grabButton = "Grab" + playerID;
        rb = GetComponent<Rigidbody>();

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
        rb.AddForce(new Vector3(Input.GetAxis(xMovementAxis)*movementSpeed, 0, -Input.GetAxis(yMovementAxis)*movementSpeed));
        }
        if (Input.GetButton(jumpButton) && rb.transform.position.y < 0.7f)
        {
            Debug.Log("jumping");
            canJump = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
        if (Input.GetButton(grabButton) && grabbableObjects.Count > 0 && !grabbing) {
            Debug.Log("grab object");
            grabbingPoint.transform.position = grabbableObjects[0].transform.position;


            //Rigidbody enemyRb;
            //enemyRb = grabbableObjects[0].GetComponent<Rigidbody>();
            //enemyRb.mass = 0.1f;
            //enemyRb.velocity = new Vector3(0,0,0);
            //enemyRb.isKinematic = true;
            //grabbableObjects[0].transform.SetParent(cylinder.transform);

            //grabbableObjects[0].GetComponent<EnemyBehaviour>().grabbed = true;

            Rigidbody enemyRb;
            enemyRb = grabbableObjects[0].GetComponent<Rigidbody>();

            grabbing = true;



        }

        if (grabbing)
        {
            grabbableObjects[0].transform.position = grabbingPoint.transform.position;
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
