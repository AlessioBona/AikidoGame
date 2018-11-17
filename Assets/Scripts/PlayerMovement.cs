using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public string playerID;
    public GameObject cylinder;
    public GameObject grabbingPoint;

    public GameObject grabbedObject;

    string xMovementAxis;
    string yMovementAxis;
    string jumpButton;
    string grabButton;
    Quaternion directionTo;

    Rigidbody rb;
    public float movementSpeed;
    public float jumpForce;
    public float rotationSpeed;
    public float pushForce;

    public List<GameObject> grabbableObjects;

    [System.Serializable]
    public struct State
    {
        public bool canJump;
        public bool grabbing;

        public State(bool p1, bool p2)
        {
            canJump = p1;
            grabbing = p2;
        }
    }
    public State state = new State(true, false);

    [System.Serializable]
    public struct Spin
    {
        public float lastDirection;
        public float value;
        public float level1;
        public float level2;
        public float limit;
        public float mount;
        public float drop;
        public float forceRatio;
        public float rotationSpeedRatio;
        public float emissionRatio;
        public ParticleSystem particles;
    }
    public Spin spin;

    private void Awake()
    {
        xMovementAxis = "Horizontal" + playerID;
        yMovementAxis = "Vertical" + playerID;
        jumpButton = "Jump" + playerID;
        grabButton = "Grab" + playerID;
        rb = GetComponentInChildren<Rigidbody>();
        directionTo = cylinder.transform.rotation;
    }


    void Update()
    {
        grabUpdate();
    }

    private void FixedUpdate()
    {

        if (Input.GetAxis(xMovementAxis) != 0 || Input.GetAxis(yMovementAxis) != 0)
        {
            rb.AddForce(
                new Vector3(
                    Input.GetAxis(xMovementAxis) * movementSpeed,
                    0,
                    -Input.GetAxis(yMovementAxis) * movementSpeed
                )
            );
            directionTo = Quaternion.Euler(
                new Vector3(
                    0,
                    Mathf.Atan2(Input.GetAxis(xMovementAxis),
                                -Input.GetAxis(yMovementAxis)) * 180 / Mathf.PI,
                    0)
            );
        }

        if (Input.GetButton(jumpButton) && state.canJump)
        {

            Debug.Log("jumping");
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }

        grabFixedUpdate(Input.GetButton(grabButton));
    }

    void grabUpdate()
    {

        if (spin.particles.isPlaying && Mathf.Abs(spin.value) < spin.level1)
        {
            spin.particles.Stop();
        }
        if (!spin.particles.isPlaying && Mathf.Abs(spin.value) > spin.level1)
        {
            spin.particles.Play();
        }

        if (state.grabbing)
        {
            float spinDifference = spin.lastDirection -
                cylinder.transform.rotation.eulerAngles.y;
            if (spinDifference < 200 && spinDifference > -200)
            {
                spin.value += spinDifference * spin.mount;
                var emission = spin.particles.emission;
                emission.rateOverTime = Mathf.Abs(spin.value) / spin.emissionRatio;
            }
            spin.lastDirection = cylinder.transform.rotation.eulerAngles.y;
        }

        if (spin.value != 0)
        {
            if (spin.value > spin.drop * Time.deltaTime)
            {
                spin.value -= spin.drop * Time.deltaTime;
            }
            if (spin.value < spin.drop * Time.deltaTime)
            {
                spin.value += spin.drop * Time.deltaTime;
            }
        }
    }

    void grabFixedUpdate(bool grabButtonPressed)
    {
        if (directionTo != cylinder.transform.rotation)
        {
            cylinder.transform.rotation = Quaternion.Slerp(
                cylinder.transform.rotation,
                directionTo,
                Time.deltaTime * (rotationSpeed + spin.value / spin.rotationSpeedRatio));
        }

        cylinder.transform.position = rb.transform.position;

        spin.particles.transform.position = new Vector3(
            rb.transform.position.x, 0.1f, rb.transform.position.z
        );

        if (grabButtonPressed && grabbableObjects.Count > 0 && !state.grabbing)
        {
            Debug.Log("grab object");
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
            Vector3 relativePos = grabbedObject.transform.position -
                                                     cylinder.transform.position;
            relativePos.y = 0;
            cylinder.transform.rotation = Quaternion.LookRotation(relativePos);
            grabbingPoint.transform.position = grabbedObject.transform.position;

            spin.lastDirection = cylinder.transform.rotation.eulerAngles.y;
            state.grabbing = true;
        }

        if (state.grabbing)
        {
            Vector3 actualXZ = new Vector3(
                grabbingPoint.transform.position.x,
                grabbedObject.transform.position.y,
                grabbingPoint.transform.position.z
            );
            grabbedObject.transform.position = actualXZ;
        }

        if (!grabButtonPressed && state.grabbing)
        {
            state.grabbing = false;
            Vector3 pushDir = cylinder.transform.forward * 50;
            grabbedObject.GetComponent<Rigidbody>().AddForce(
                pushDir * (pushForce + (Mathf.Abs(spin.value) / spin.forceRatio))
            );
            spin.value = 0;
            grabbedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
