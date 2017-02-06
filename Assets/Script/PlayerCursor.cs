using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour {

	public Texture2D defaultTexture; 
	public CursorMode curMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	public GameObject bombClass;
	public GameObject boxObstacle;
	public UnityEngine.UI.Text player1Power;
	public float currentPower = 100.0f;
	public float rechargeRate = 3.0f;
	public float boxCost = 15.0f;
	public float bombCost = 10.0f;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 currMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (currentPower < 100.0f) {
			currentPower = Mathf.Min (100.0f, currentPower + (Time.deltaTime * rechargeRate));
		}
		player1Power.text = "Current Power: " + currentPower.ToString ();
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Pressed left click.");
			if (currentPower >= bombCost) {
				GameObject go = Instantiate (bombClass, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity) as GameObject; 
				currentPower = currentPower - bombCost;
			}
		}
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log("Pressed right click.");
			if (currentPower >= boxCost) {
				Debug.Log ("Pressed left click.");
				GameObject go = Instantiate (boxObstacle, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity) as GameObject; 
				currentPower = currentPower - boxCost;
			}
		}

		if (Input.GetMouseButtonDown(2))
			Debug.Log("Pressed middle click.");
	}

	void OnMouseEnter(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}

	void OnMouseExit(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}
}
