using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyBehaviour : MonoBehaviour {

    public EnemyData myData;

	// Use this for initialization
	void Start () {
        myData = GetComponent<EnemyData>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        
    }


}
