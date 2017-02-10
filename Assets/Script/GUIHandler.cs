using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {

	public static GUIHandler instance = null;
	[TextArea(1,10)]
	public string textMessage = "";
	private bool displayTextMessage = false;
	private float displayTime;
	private float displayStart;
	private float displayTimePassed;

	private GameManager gameManager;


//	void Start() {
//		
////		textMessage = new Text ();
//		textMessage.text = "Hello";
////		p1 = FindObjectOfType<Player> ();
////		p1controller = p1.gameObject.GetComponent<Controller2D> ();
////		Debug.Log (p1controller);
//	}
	void Awake () {
		Debug.Log ("Awake");
		if (instance == null)
			instance = this;
		else if (instance != this) {
			Destroy (gameObject);
		}
		gameManager = FindObjectOfType<GameManager> ();
	}

	void Update() {
//		if (p1controller.gameOver) {
//			displayText ("Player " + p1controller.winner + " wins!", 3f);
//			p1controller.gameOver = false;
//			p1controller.winner = 0;
//		}
//		displayText ("Player " + p1controller.winner + " wins!", 3f);
		if (gameManager.gameOver) {
			displayText ("Player " + gameManager.winner + " wins!", 3f);
			gameManager.gameOver = false;
			gameManager.winner = 0;
		}

		if (displayTextMessage) {
			if (displayTimePassed < displayTime) {
				displayTimePassed = Time.time - displayStart;
			} else {
				displayTextMessage = false;
				textMessage = "";
			}
		}
	}

	public void displayText(string msg, float dTime) {
		displayTextMessage = true;
		textMessage = msg;
		displayTime = dTime;
		displayStart = Time.time;
		displayTimePassed = 0f;
	}

	void OnGUI() {
		if (displayTextMessage) {
			Debug.Log (Screen.width + ", " + Screen.height);
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.fontSize = Screen.width / 40;
			centeredStyle.alignment = TextAnchor.UpperCenter;
			int w = 200;
			int h = 50;
			GUI.Label (new Rect (Screen.width/2-w/2, Screen.height/2-h/2, w, h), textMessage, centeredStyle);
		}
	}
}