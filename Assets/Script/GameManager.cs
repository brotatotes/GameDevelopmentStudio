using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int winner = 0; // 0 for no winner, 1 for player 1, 2 for player 2
	public bool gameOver = false;
	public string lastMouseButtonPressed = "Left Button";
	public Player Player1;
	List<GameObject> godPowers = new List<GameObject>();
	public GameObject prefabButton;
	public GameObject prefabText;
	public GameObject playerCursorPrefab;
	GameObject godCursor;
	GameObject curPlayer;
//	GameObject playerHealthUI;
//	GameObject godPowerUI;
	bool foundPlayer;

	public GameObject playerHealth;
	public GameObject godPower;
	public Dictionary<string, Button> allButtons;
	public Dictionary<string, Spawnable> allPowers;
	public List<GodButtons> godButtons = new List<GodButtons> (); 
	public int currIndex = 0;
	public int maxButtons = 0;

	public float startX; // used by PlayerCursor for deadZone

	void Awake () {
//		Debug.Log ("Awake");
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
		godCursor = (GameObject)Instantiate (playerCursorPrefab);

//		Debug.Log ("init game");
		Object[] allObjs = Resources.LoadAll ("");
		allButtons = new Dictionary<string, Button> ();
		allPowers = new Dictionary<string, Spawnable> ();
//		float xPos = 0.0f;
		float xPos = Screen.width - allObjs.Length * 50.0f;
		startX = xPos;
		float maxX = 50.0f;
		foreach (Object obj in allObjs) {
			GameObject go = (GameObject)obj;

			if (go.GetComponent<Spawnable> ()) {
				godPowers.Add (go);
				Spawnable spawnInfo = go.GetComponent<Spawnable> ();
				GameObject buttonObj = (GameObject)Instantiate (prefabButton);
				Button tempButton = buttonObj.GetComponent<Button> ();

				buttonObj.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform);
				buttonObj.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, 0.0f);
				tempButton.GetComponentsInChildren<Text> () [0].text = spawnInfo.name;
				buttonObj.GetComponent<GodButtons> ().godCursor = godCursor;
				buttonObj.GetComponent<GodButtons> ().spawnObj = go;
				maxButtons = maxButtons + 1;
				godButtons.Add (buttonObj.GetComponent<GodButtons> ());
				buttonObj.GetComponent<GodButtons> ().buttonID = maxButtons;
				if (!godCursor.GetComponent<PlayerCursor> ().initLeft) {
					godCursor.GetComponent<PlayerCursor> ().leftObj = go;
					godCursor.GetComponent<PlayerCursor> ().initLeft = true;
				} else if (!godCursor.GetComponent<PlayerCursor> ().initRight) {
					godCursor.GetComponent<PlayerCursor> ().rightObj = go;
					godCursor.GetComponent<PlayerCursor> ().initRight = true;
				}

				GameObject textObj = (GameObject)Instantiate (prefabText);
				Text tempText = textObj.GetComponent<Text> ();
				textObj.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform);
				textObj.GetComponent<RectTransform> ().transform.position = 
					new Vector3 (tempButton.transform.position.x+25f, tempButton.transform.position.y-38f);
				tempText.text = spawnInfo.cost.ToString ();


				allButtons.Add (spawnInfo.name, tempButton);
				allPowers.Add (spawnInfo.name, spawnInfo);

				xPos += 50.0f;
				maxX += 50.0f;
			}
		}
		godCursor.GetComponent<PlayerCursor> ().deadX = maxX;
//		godPowerUI = (GameObject)Instantiate (godPower);
//		godPowerUI.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform);
//		godPowerUI.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.0f, -45.0f);
	}
		
	// Update is called once per frame
	void Update () {
		if (!foundPlayer) {
			curPlayer = GameObject.FindGameObjectWithTag("Player") as GameObject;
			foundPlayer = true;
//			playerHealthUI = (GameObject)Instantiate (playerHealth);
//			playerHealthUI.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform);
//			playerHealthUI.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.0f, -60.0f);
		} else {
//			playerHealthUI.GetComponent<Text>().text = "Player Health: " + curPlayer.GetComponent<Controller2D>().health.ToString ();
		}
//		godPowerUI.GetComponent<Text>().text = "Current Power: " + godCursor.GetComponent<PlayerCursor>().currentPower.ToString ();

		if (Input.GetMouseButton (0))
			lastMouseButtonPressed = "Left button";
		if (Input.GetMouseButton (1))
			lastMouseButtonPressed = "Right button";		
	}
}
