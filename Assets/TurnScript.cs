using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScript : MonoBehaviour {
    private GameManager gameMan;
    TextMesh turnText;
    int turn;
    bool skip;
	// Use this for initialization
	void Start () {
        turnText = GetComponent<TextMesh>();
        GameObject manager = GameObject.Find("GameManager");
        gameMan = manager.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        turn = gameMan.playerSide;
        skip = gameMan.skipTurn;
        if (turn == 1)
        {
            turnText.text = "Player Turn";
        }
        else
        {
            turnText.text = "Enemy Turn";
        }
	}
}
