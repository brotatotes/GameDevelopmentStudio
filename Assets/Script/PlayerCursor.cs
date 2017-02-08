using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour {

	public Texture2D defaultTexture; 
	public CursorMode curMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
//	public GameObject bombClass;
//	public GameObject boxObstacle;
//	public GameObject fanItem;

	public float currentPower = 100.0f;
	public float rechargeRate = 3.0f;
//	public float boxCost = 15.0f;
//	public float bombCost = 10.0f;
//	public float fanCost = 10.0f;

	public GameObject leftObj;
	public bool initLeft = false;
	public GameObject rightObj;
	public bool initRight = false;
	public float deadX = 0.0f;
	float deadY = 30.0f;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);
	}
	
	// Update is called once per frame
	void Update() {
		if (currentPower < 100.0f) {
			currentPower = Mathf.Min (100.0f, currentPower + (Time.deltaTime * rechargeRate));
		}
		if (Input.mousePosition.y < (Screen.height - deadY) || Input.mousePosition.x > deadX) {
			Vector3 currMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("Pressed left click.");
				float cost = leftObj.GetComponent<Spawnable> ().cost;
				if (currentPower >= cost) {
					Debug.Log (Resources.Load ("Bomb"));
					Instantiate (leftObj, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
					//Instantiate (bombClass, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
					currentPower = currentPower - cost;
				}
			}
			if (Input.GetMouseButtonDown (1)) {
				Debug.Log ("Pressed right click.");
				float cost = rightObj.GetComponent<Spawnable> ().cost;
				if (currentPower >= cost) {
					Instantiate (rightObj, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
					currentPower = currentPower - cost;
				}
			}
		}
		/*
		if (Input.GetMouseButtonDown (2)) {
			Debug.Log ("Pressed middle click.");
			Instantiate (fanItem, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
			currentPower = currentPower - fanCost;
		}*/
	}

	void OnMouseEnter(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}

	void OnMouseExit(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}
}
