using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {

	public static GUIHandler instance = null;
	[TextArea(1,10)]
	public string textMessage = "";

	public Slider P1HealthBar;
	public Slider P1EnergyBar;
	public Slider P2EnergyBar;
	public Image P2EnergyBarFill;

	public Dictionary<string, Button> allButtons;
	public Dictionary<string, Spawnable> allPowers;

	private bool displayTextMessage = false;
	private float displayTime;
	private float displayStart;
	private float displayTimePassed;

	private bool flashRed = false;
	private float flashTime;
	private float flashStart;
	private float flashTimePassed;

	private GameManager gameManager;

	void Awake () {
//		Debug.Log ("Awake");
		if (instance == null)
			instance = this;
		else if (instance != this) {
			Destroy (gameObject);
		}
		gameManager = FindObjectOfType<GameManager> ();
		allButtons = gameManager.allButtons;
		allPowers = gameManager.allPowers;
	}

	void Update() {

		var P1 = FindObjectOfType<Player> ();
		var P1Controller = P1.GetComponent<Attackable> ();
		var P2 = FindObjectOfType<PlayerCursor> ();
		P2EnergyBar.value = P2.currentPower;

		P1EnergyBar.value = P1Controller.energy;

//		allButtons [P2.leftObj.name].GetComponent<Image> ().color = Color.cyan;
//		allButtons [P2.rightObj.name].GetComponent<Image> ().color = Color.green;
		foreach(KeyValuePair<string, Button> entry in allButtons) {
			Color buttonColor;
			string click = "";
			if (entry.Key == P2.leftObj.name) {
				buttonColor = new Color (0.4f, 0.7f, 1f);
				click = "left";
			} else if (entry.Key == P2.rightObj.name) {
				buttonColor = new Color(0.6f, 0.6f, 1f);
				click = "right";
			} else {
				buttonColor = Color.white;
			}

			if (P2.currentPower < allPowers[entry.Key].cost) {
				if (buttonColor == Color.white)
					buttonColor = Color.grey;
				else if (click == "left"){
					buttonColor = new Color (0.2f, 0.5f, 0.8f);
				} else {
					buttonColor = new Color (0.4f, 0.4f, 0.8f);
				}
			}
			entry.Value.GetComponent<Image> ().color = buttonColor;
		}

		if (gameManager.gameOver) {
			displayText ("Player " + gameManager.winner + " wins!", 3f);
			gameManager.gameOver = false;
			gameManager.winner = 0;
			P1HealthBar.value = 0;
		} else {
			P1HealthBar.value = P1Controller.health;
		}

		if (displayTextMessage) {
			if (displayTimePassed < displayTime) {
				displayTimePassed = Time.time - displayStart;
			} else {
				displayTextMessage = false;
				textMessage = "";
			}
		}

		if (flashRed) {
			if (flashTimePassed < flashTime) {
				flashTimePassed = Time.time - flashStart;
				float fTimeRatio = flashTimePassed / flashTime;
				if (fTimeRatio <= 0.25f || (fTimeRatio > 0.5f && fTimeRatio <= 0.75f)) {
					P2EnergyBarFill.color = Color.red;
				} else {
					P2EnergyBarFill.color = Color.yellow;
				}
			} else {
				flashRed = false;
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

	public void P2EnergyBarFlashRed() {
		flashRed = true;
		flashTime = 0.4f;
		flashStart = Time.time;
		flashTimePassed = 0f;
	}

	void OnGUI() {
		if (displayTextMessage) {
//			Debug.Log (Screen.width + ", " + Screen.height);
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.fontSize = Screen.width / 40;
			centeredStyle.alignment = TextAnchor.UpperCenter;
			int w = 200;
			int h = 50;
			GUI.Label (new Rect (Screen.width/2-w/2, Screen.height/2-h/2, w, h), textMessage, centeredStyle);
		}
	}
}