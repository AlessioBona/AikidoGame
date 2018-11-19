using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AleEnemyBehaviour : MonoBehaviour {

    public bool grabbed = false;
    public float charge;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Grabbed()
    {
        grabbed = true;
        anim.enabled = false;
        GetComponent<Outline>().enabled = true;
    }

    public void Ungrabbed()
    {
        grabbed = false;
        anim.enabled = true;
        GetComponent<Outline>().enabled = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMovement>().grabbableObjects
                 .Add(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMovement>().grabbableObjects
                 .Remove(gameObject);
        }
    }
}
