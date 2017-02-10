using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {

//	Player p1;
//	Controller2D p1controller;
	// display text notifiers
	public Text textMessage;
	private bool displayTextMessage = false;
	private float displayTime;
	private float displayStart;
	private float displayTimePassed;


	void Start() {
		
//		textMessage = new Text ();
		textMessage.text = "Hello";
//		p1 = FindObjectOfType<Player> ();
//		p1controller = p1.gameObject.GetComponent<Controller2D> ();
//		Debug.Log (p1controller);
	}

	void Update() {
//		if (p1controller.gameOver) {
//			displayText ("Player " + p1controller.winner + " wins!", 3f);
//			p1controller.gameOver = false;
//			p1controller.winner = 0;
//		}
//		displayText ("Player " + p1controller.winner + " wins!", 3f);

		if (displayTextMessage) {
			if (displayTimePassed < displayTime) {
				displayTimePassed = Time.time - displayStart;
			} else {
				displayTextMessage = false;
				textMessage.text = "";
			}
		}
	}

	public void displayText(string msg, float dTime) {
		displayTextMessage = true;
		textMessage.text = msg;
		displayTime = dTime;
		displayStart = Time.time;
		displayTimePassed = 0f;
	}

	void OnGUI() {
		if (displayTextMessage) {
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), textMessage.text);
		}
	}
}