using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public string playerID;
    public GameObject cylinder;
    public GameObject grabbingPoint;
    public ParticleSystem spinParticles;
    public GameObject grabbedObject;

    string xMovementAxis;
    string yMovementAxis;
    string jumpButton;
    string grabButton;
    Quaternion directionTo;

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
        rb = GetComponentInChildren<Rigidbody>();
        directionTo = cylinder.transform.rotation;

    }

    // Use this for initialization
    void Start () {
		
	}

    public float spinLastDirection;
    public float spinValue;
    public float spinLevel1;
    public float spinLevel2;
    public float spinLimit;
    public float spinMount;
    public float spinDrop;

    public float spinForceRatio;
    public float spinRotationSpeedRatio;
    public float spinEmissionRatio;

	// Update is called once per frame
	void Update () {

        if(spinParticles.isPlaying && Mathf.Abs(spinValue) < spinLevel1)
        {
            spinParticles.Stop();
        }
        if(!spinParticles.isPlaying && Mathf.Abs(spinValue) > spinLevel1)
        {
            spinParticles.Play();
        }

        if (grabbing)
        {
            float spinDifference  = spinLastDirection - cylinder.transform.rotation.eulerAngles.y;
            if (spinDifference < 200 && spinDifference > -200)
            {
                spinValue += spinDifference*spinMount;
                var emission = spinParticles.emission;
                emission.rateOverTime = Mathf.Abs(spinValue) / spinEmissionRatio;
            }
            spinLastDirection = cylinder.transform.rotation.eulerAngles.y;
        }

        if(spinValue != 0)
        {
            if(spinValue > spinDrop*Time.deltaTime)
            {
                spinValue -= spinDrop*Time.deltaTime;
            }
            if(spinValue < spinDrop*Time.deltaTime)
            {
                spinValue += spinDrop*Time.deltaTime;
            }
        }
	}

    private void FixedUpdate()
    {
        


        if (Input.GetAxis(xMovementAxis) != 0 || Input.GetAxis(yMovementAxis) != 0) { 
        rb.AddForce(new Vector3(Input.GetAxis(xMovementAxis)*movementSpeed, 0, -Input.GetAxis(yMovementAxis)*movementSpeed));
            if (Mathf.Abs(Input.GetAxis(xMovementAxis)) > 0.7f || Mathf.Abs(Input.GetAxis(yMovementAxis)) > 0.7f)
            {
                directionTo = Quaternion.Euler(new Vector3(0, Mathf.Atan2(Input.GetAxis(xMovementAxis), -Input.GetAxis(yMovementAxis)) * 180 / Mathf.PI, 0));
            }
        }



        if (directionTo != cylinder.transform.rotation)
        {
            cylinder.transform.rotation = Quaternion.Slerp(cylinder.transform.rotation, directionTo, Time.deltaTime * (rotationSpeed + spinValue / spinRotationSpeedRatio));
        }

        cylinder.transform.position = new Vector3(rb.transform.position.x, 0.1f, rb.transform.position.z);
        spinParticles.transform.position = new Vector3(rb.transform.position.x, 0.1f, rb.transform.position.z);





        if (Input.GetButton(jumpButton) && rb.transform.position.y < 0.7f)
        {
            canJump = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }

        // try to grab
        if (Input.GetButton(grabButton) && grabbableObjects.Count > 0 && !grabbing) {

            GameObject toBeGrabbed = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = rb.transform.position;
            foreach (GameObject enemy in grabbableObjects)
            {
                float dist = Vector3.Distance(enemy.transform.position, currentPos);
                if (dist < minDist)
                {
                    toBeGrabbed = enemy;
                    minDist = dist;
                }
            }
            grabbedObject = toBeGrabbed;

            Vector3 relativePos = grabbedObject.transform.position - cylinder.transform.position;
            relativePos.y = 0;
            cylinder.transform.rotation = Quaternion.LookRotation(relativePos);
            grabbingPoint.transform.position = grabbedObject.transform.position;


            //Rigidbody enemyRb;
            //enemyRb = grabbedObject.GetComponent<Rigidbody>();
            //enemyRb.mass = 0.1f;
            //enemyRb.velocity = new Vector3(0,0,0);
            //enemyRb.isKinematic = true;
            //grabbedObject.transform.SetParent(cylinder.transform);

            //grabbedObject.GetComponent<EnemyBehaviour>().grabbed = true;

            spinLastDirection = cylinder.transform.rotation.eulerAngles.y;
            grabbing = true;
        }

        if (grabbing)
        {
            Vector3 actualXZ = new Vector3(grabbingPoint.transform.position.x, grabbedObject.transform.position.y, grabbingPoint.transform.position.z);
            grabbedObject.transform.position = actualXZ;
        }

        if (!Input.GetButton(grabButton) && grabbing)
        {
            //let the grab
            grabbing = false;
            Vector3 pushDir = cylinder.transform.forward * 50;
            // or cylinder.transform.forward*80;
            // or new Vector3(Input.GetAxis(xMovementAxis), 0, -Input.GetAxis(yMovementAxis)).normalized*100
            grabbedObject.GetComponent<Rigidbody>().AddForce(pushDir * (pushForce + (Mathf.Abs(spinValue) /spinForceRatio)));
            spinValue = 0;
            grabbedObject = null;
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
