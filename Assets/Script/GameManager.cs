using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int winner = 0; // 0 for no winner, 1 for player 1, 2 for player 2
	public bool gameOver = false;

	// Use this for initialization
	void Start () {
		winner = 0;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
