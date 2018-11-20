using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyData : MonoBehaviour {

    public string enemyName;
    public float damagePerHit;
    public float startHealth;
    public float actualHealth;

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

        // changeText
        if (GetComponentInChildren<TextMeshProUGUI>())
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Health: " + actualHealth.ToString();
        }
    }

}
