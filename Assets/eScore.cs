using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eScore : MonoBehaviour {
    private GameManager gameMan;
    TextMesh scoreText;
    int score;

    // Use this for initialization
    void Start () {
        scoreText = GetComponent<TextMesh>();
        score = 0;
        GameObject manager = GameObject.Find("GameManager");
        gameMan = manager.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        score = gameMan.oppositeScore;
        scoreText.text = "Enemy: " + score;
    }
}
