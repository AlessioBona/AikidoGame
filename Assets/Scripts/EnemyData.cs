using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyData : MonoBehaviour {

    public string enemyName;
    public float damagePerHit;
    public float startHealth;
    public float actualHealth;
    public GameObject deathExplosion;
    public GameObject impactFX;

	// Use this for initialization
	void Start () {
        actualHealth = startHealth;

        // change Text
        if (GetComponentInChildren<TextMeshProUGUI>())
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Health: " + actualHealth.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void ChangeHealth(float change)
    {
        actualHealth += change;

        if(actualHealth <= 0)
        {
            Death();
        }

        // changeText
        if (GetComponentInChildren<TextMeshProUGUI>())
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Health: " + actualHealth.ToString();
        }
    }

    public void Death()
    {
        Instantiate(deathExplosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
        GameObject.Destroy(this.gameObject);
    }

}
