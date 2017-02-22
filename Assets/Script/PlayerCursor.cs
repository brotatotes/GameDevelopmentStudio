using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerCursor : NetworkBehaviour {

	public Texture2D defaultTexture; 
	public Texture2D leftFull;
	public Texture2D rightFull;
	public Texture2D noneFull;

	public CursorMode curMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
//	public GameObject bombClass;
//	public GameObject boxObstacle;
//	public GameObject fanItem;

	public float currentPower = 100.0f;
	public float rechargeRate = 3.0f;
	public bool notEnoughEnergy = false;
//	public float boxCost = 15.0f;
//	public float bombCost = 10.0f;
//	public float fanCost = 10.0f;

	public GameObject leftObj;
	public bool initLeft = false;
	public GameObject rightObj;
	public bool initRight = false;
	public float deadX = 0.0f;
	float deadY = 30.0f;
	float toCreateL = 0f;
	float toCreateR = 0f;
	Vector3 initDownL;
	Vector3 initDownR;
	GameManager gm;


	// Use this for initialization
	void Start () {
		//Cursor.SetCursor(defaultTexture, hotSpot, curMode);
		//deadX = FindObjectOfType<GameManager> ().startX;
		//gm = FindObjectOfType<GameManager> ();
	}

	void checkCursor(float currentPower, float left_cost, float right_cost){
		if (currentPower < left_cost && currentPower >= right_cost){
			Cursor.SetCursor(rightFull, hotSpot, curMode);
		}
		if (currentPower >= left_cost &&  currentPower < right_cost){
			Cursor.SetCursor(leftFull, hotSpot, curMode);
		}
		if (currentPower < left_cost && currentPower < right_cost){
			Cursor.SetCursor(noneFull, hotSpot, curMode);
		}
		if (currentPower >= left_cost && currentPower >= right_cost){
			Cursor.SetCursor(defaultTexture, hotSpot, curMode);
		}
	}

	// Update is called once per frame
	void Update() {
		
	}
	/*
	[ClientRpc]
	public void RpcCreateObject(string mouseButton, Vector3 currMousePos, Vector2 angleDiff)
	{
		Debug.Log ("RPC Spawning Obj");

		GameObject spawnObj;
		if (mouseButton == "left") {
			spawnObj = leftObj;
		} else {
			spawnObj = rightObj;
		}
		GameObject newObj = Instantiate (spawnObj, GetPlacePos(leftObj,currMousePos), Quaternion.identity);
		newObj.GetComponent<Spawnable> ().angleDiff = angleDiff;
		currentPower = currentPower - newObj.GetComponent<Spawnable>().cost;
		toCreateR = 0f;
		Debug.Log (newObj);
		Debug.Log (newObj.GetComponent<Attackable>().netId);
		NetworkServer.Spawn (newObj);
	}*/
}
