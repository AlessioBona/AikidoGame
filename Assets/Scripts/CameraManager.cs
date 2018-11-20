using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager cFollow;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    public float offX;
    public float offZ;

    public float zoomRatio;
    float zoom = 0;

    public float MidX;
    public float MidZ;
    public Transform target1;
    public Transform target2;
    public Vector3 Midpoint;
    public Vector3 distance;
    public float camDistance;
    public float CamOffset;
    public float bounds;

    Camera cam;
    public GameObject cameraObject;
    public Vector3 playerDistance;
    public float distanceTolerance;

    void Awake()
    {

        //checking if there's already a game manager
        if (cFollow == null)
        {
            DontDestroyOnLoad(gameObject);
            cFollow = this;
        }
        else if (cFollow != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        offX = transform.position.x;
        offZ = transform.position.z;
        zoom = cam.fieldOfView;
        playerDistance = target2.position - target1.position;

    }

    // Update is called once per frame
    void Update()
    {
        MidX = (target2.position.x + target1.position.x) / 2;
        //MidY = (target2.position.y + target1.position.y) / 2;
        MidZ = (target2.position.z + target1.position.z) / 2;

        playerDistance = target2.position - target1.position;

        float linearDistance = Mathf.Abs(playerDistance.x) + Mathf.Abs(playerDistance.z);
        if (linearDistance > distanceTolerance) {
            cam.fieldOfView = zoom + (linearDistance-distanceTolerance) * zoomRatio;
        }


        //distance = target1.position - target2.position;

        //if (target1.position.x < (transform.position.x - bounds))
        //{
        //    Vector3 pos = target1.position;
        //    pos.x = transform.position.x - bounds;
        //    target1.position = pos;
        //}
        //if (target2.position.x < (transform.position.x - bounds))
        //{
        //    Vector3 pos = target2.position;
        //    pos.x = transform.position.x - bounds;
        //    target2.position = pos;
        //}
        //if (target1.position.x > (transform.position.x + bounds))
        //{
        //    Vector3 pos = target1.position;
        //    pos.x = transform.position.x + bounds;
        //    target1.position = pos;
        //}
        //if (target2.position.x > (transform.position.x + bounds))
        //{
        //    Vector3 pos = target2.position;
        //    pos.x = transform.position.x + bounds;
        //    target2.position = pos;
        //}
        //if (distance.x > 15.0f)
        //{
        //    CamOffset = distance.x * 0.3f;
        //    if (CamOffset >= 8.5f)
        //        CamOffset = 8.5f;
        //}
        //else if (distance.x < 14.0f)
        //{
        //    CamOffset = distance.x * 0.3f;
        //}
        //else if (distance.z < 14.0f)
        //{
        //    CamOffset = distance.x * 0.3f;
        //}

        Midpoint = new Vector3(MidX + offX, transform.position.y, MidZ + offZ);


        //Vector3 point = GetComponentInChildren<Camera>().WorldToViewportPoint(Midpoint);
        //Vector3 delta = Midpoint - transform.position;
        //Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, Midpoint, ref velocity, dampTime);

    }
}
