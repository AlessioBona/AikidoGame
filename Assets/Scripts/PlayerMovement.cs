﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public string playerID;
    public GameObject cylinder;
    public GameObject grabbingPoint;

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

    [System.Serializable]
    public struct Spin {
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
    
    void Update()
    {
        Debug.Log(spin.value);

        if (spin.particles.isPlaying && Mathf.Abs(spin.value) < spin.level1)
        {
            spin.particles.Stop();
        }
        if (!spin.particles.isPlaying && Mathf.Abs(spin.value) > spin.level1)
        {
            spin.particles.Play();
        }

        if (grabbing)
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

        if (directionTo != cylinder.transform.rotation)
        {
            cylinder.transform.rotation = Quaternion.Slerp(
                cylinder.transform.rotation,
                directionTo,
                Time.deltaTime * (rotationSpeed + spin.value / spin.rotationSpeedRatio));
        }

        cylinder.transform.position = new Vector3(
            rb.transform.position.x, 0.1f, rb.transform.position.z
        );
        spin.particles.transform.position = new Vector3(
            rb.transform.position.x, 0.1f, rb.transform.position.z
        );

        if (Input.GetButton(jumpButton) && rb.transform.position.y < 0.7f)
        {
            Debug.Log("jumping");
            canJump = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
        if (Input.GetButton(grabButton) && grabbableObjects.Count > 0 && !grabbing)
        {
            Debug.Log("grab object");
            Vector3 relativePos = grabbableObjects[0].transform.position -
                                                     cylinder.transform.position;
            relativePos.y = 0;
            cylinder.transform.rotation = Quaternion.LookRotation(relativePos);
            grabbingPoint.transform.position = grabbableObjects[0].transform.position;

            spin.lastDirection = cylinder.transform.rotation.eulerAngles.y;
            grabbing = true;
        }

        if (grabbing)
        {
            Vector3 actualXZ = new Vector3(
                grabbingPoint.transform.position.x,
                grabbableObjects[0].transform.position.y,
                grabbingPoint.transform.position.z
            );
            grabbableObjects[0].transform.position = actualXZ;
        }

        if (!Input.GetButton(grabButton) && grabbing)
        {
            grabbing = false;
            Vector3 pushDir = cylinder.transform.forward * 50;
            Debug.Log(pushDir);
            grabbableObjects[0].GetComponent<Rigidbody>().AddForce(
                pushDir * (pushForce + (Mathf.Abs(spin.value) / spin.forceRatio))
            );
            spin.value = 0;
        }
    }
}
