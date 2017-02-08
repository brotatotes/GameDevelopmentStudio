using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int winner = 0; // 0 for no winner, 1 for player 1, 2 for player 2
	public bool gameOver = false;
	public Player Player1;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		winner = 0;
		gameOver = false;
		InitGame ();
	}

	void InitGame() {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
