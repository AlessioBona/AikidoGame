using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class EnemyBehaviour : MonoBehaviour
{
    public bool grabbed = false;
    public NavMeshAgent agent;
    public GameObject[] targets;

    private void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!grabbed)
        {
            agent.SetDestination(ClosestPlayer().transform.position);
        }
    }

    private GameObject ClosestPlayer()
    {
        Dictionary<GameObject, float> tDict;
        tDict = targets.ToDictionary(target => target, target => Vector3.Distance(
            target.transform.position,
            transform.position
        ));
        GameObject closestObject = tDict.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;

        return closestObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMovement>().grabbableObjects
                 .Add(gameObject);
            grabbed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMovement>().grabbableObjects
                 .Remove(gameObject);
            grabbed = false;
        }
    }
}
