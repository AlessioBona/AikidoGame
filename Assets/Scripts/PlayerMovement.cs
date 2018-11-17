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
    public float rotationSpeed;
    public float pushForce;
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
        if (Input.GetAxis(xMovementAxis) != 0 || Input.GetAxis(yMovementAxis) != 0) { 
        rb.AddForce(new Vector3(Input.GetAxis(xMovementAxis)*movementSpeed, 0, -Input.GetAxis(yMovementAxis)*movementSpeed));
            Quaternion directionTo = Quaternion.Euler(new Vector3(0, Mathf.Atan2(Input.GetAxis(xMovementAxis), -Input.GetAxis(yMovementAxis)) * 180 / Mathf.PI, 0));
        cylinder.transform.rotation = Quaternion.Slerp(cylinder.transform.rotation, directionTo, Time.deltaTime*rotationSpeed);
        
        }


        if (Input.GetButton(jumpButton) && rb.transform.position.y < 0.7f)
        {
            Debug.Log("jumping");
            canJump = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
        if (Input.GetButton(grabButton) && grabbableObjects.Count > 0 && !grabbing) {
            Debug.Log("grab object");
            Vector3 relativePos = grabbableObjects[0].transform.position - cylinder.transform.position;
            relativePos.y = 0;
            cylinder.transform.rotation = Quaternion.LookRotation(relativePos);
            grabbingPoint.transform.position = grabbableObjects[0].transform.position;


            //Rigidbody enemyRb;
            //enemyRb = grabbableObjects[0].GetComponent<Rigidbody>();
            //enemyRb.mass = 0.1f;
            //enemyRb.velocity = new Vector3(0,0,0);
            //enemyRb.isKinematic = true;
            //grabbableObjects[0].transform.SetParent(cylinder.transform);

            //grabbableObjects[0].GetComponent<EnemyBehaviour>().grabbed = true;


            grabbing = true;
        }

        if (grabbing)
        {
            Vector3 actualXZ = new Vector3(grabbingPoint.transform.position.x, grabbableObjects[0].transform.position.y, grabbingPoint.transform.position.z);
            grabbableObjects[0].transform.position = actualXZ;
        }

        if (!Input.GetButton(grabButton) && grabbing)
        {
            //let the grab
            grabbing = false;
            Vector3 pushDir = cylinder.transform.forward * 50;
            Debug.Log(pushDir);
            // or cylinder.transform.forward*80;
            // or new Vector3(Input.GetAxis(xMovementAxis), 0, -Input.GetAxis(yMovementAxis)).normalized*100
            grabbableObjects[0].GetComponent<Rigidbody>().AddForce(pushDir * pushForce);
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
