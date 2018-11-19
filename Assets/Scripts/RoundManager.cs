using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour {

    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    public GameObject player1;
    public GameObject player2;

    public float startHealth;

    public float player1Health;
    public float player2Health;

    // Use this for initialization
    void Start () {
        player1Health = startHealth;
        player2Health = startHealth;
        UpdateText();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateText()
    {
        player1Text.text = "Player 1 Health: " + player1Health;
        player2Text.text = "Player 2 Health: " + player2Health;
    }

    public void ChangeHealth(string player, float change) {
        Debug.Log("ok");
        if(player == "1")
        {
            player1Health += change;
        }
        if(player == "2")
        {
            player2Health += change;
        }
        UpdateText();
    }



}
