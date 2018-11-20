using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AleEnemyBehaviour : MonoBehaviour {

    public bool grabbed = false;
    public float chargeTH;
    public bool thrown = false;
    public float recoverTime;
    public float timeRecovered;
    public float charge;
    public float selfCollisionDamage;
    public float otherCollisionDamage;
    Animator anim;

    EnemyData myData;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
        myData = GetComponent<EnemyData>();
	}
	
	// Update is called once per frame
	void Update () {
        if (thrown)
        {
            timeRecovered += Time.deltaTime;
            if(timeRecovered > recoverTime)
            {
                thrown = false;
                anim.enabled = true;
                charge = 0;
            }
        }
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
        GetComponent<Outline>().enabled = false;
        if (charge >= chargeTH)
        {
            otherCollisionDamage = 35f; // TO BE CHANGED!!!
            selfCollisionDamage = 35f; // TO BE CHANGED !!!
            thrown = true;
            timeRecovered = 0f;
        }
        
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyData>() && !grabbed && charge > 0)
        {
            Instantiate(myData.impactFX, collision.contacts[0].point, collision.gameObject.transform.rotation);
            myData.ChangeHealth(-selfCollisionDamage);
            collision.gameObject.GetComponentInParent<EnemyData>().ChangeHealth(-otherCollisionDamage);
        }
    }
}
