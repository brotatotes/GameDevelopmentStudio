using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerCursor : NetworkBehaviour {
	/*
	public Texture2D defaultTexture; 
	public Texture2D leftFull;
	public Texture2D rightFull;
	public Texture2D noneFull;
*/
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
		Debug.Log("Initializing");
		Debug.Log (netId);
		deadX = FindObjectOfType<GameManager> ().startX;
		gm = FindObjectOfType<GameManager> ();
		leftObj = gm.godPowers [0];
		rightObj = gm.godPowers [1];
	}
	[Command]
	public void CmdTurnToGod() {
		Debug.Log ("Command Turn to God");
		RpcTurnToGod ();
	}
	[ClientRpc]
	void RpcTurnToGod() {
		Debug.Log ("Attempting to remove stuff");
		Destroy (GetComponent<SpriteRenderer> ());
		Destroy (GetComponent<Player>());
		Destroy (GetComponent<Fighter> ());
		Destroy (GetComponent<Attackable> ());
		FindObjectOfType<GUIHandler> ().GodTarget = this;
	}

	/*
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
	}*/

	// Update is called once per frame
	void Update() {	
		if (currentPower < 100.0f) {
			currentPower = Mathf.Min (100.0f, currentPower + (Time.deltaTime * rechargeRate));
		}
	}


	public void setIndex(string mouseButton, int index) {
		//Debug.Log ("setting Index");
		if (!isLocalPlayer)
			return;
		//Debug.Log ("Is Local Player");
		CmdSetIndex (mouseButton, index);
	}

	[Command]
	public void CmdSetIndex(string mouseButton, int index) {
		RpcSetIndex (mouseButton, index);
	}
	[ClientRpc]
	public void RpcSetIndex(string mouseButton, int index) {
		if (mouseButton == "left") {
			leftObj = gm.godPowers [index];
		} else {
			rightObj = gm.godPowers [index];
		}
	}
	/*[Command]
	public void CmdCreateObject(string mouseButton, Vector3 currMousePos, Vector2 angleDiff)
	{
		Debug.Log ("Command for RPC");
		RpcCreateObject (mouseButton, currMousePos, angleDiff);
	}*/
	[Command]
	public void CmdCreateObject(string mouseButton, Vector3 currMousePos, Vector2 angleDiff)
	{
		if (!isServer)
			return;
		Debug.Log ("Cmd Spawning Obj");
		Debug.Log (netId);

		RpcCreateObject(mouseButton,currMousePos, angleDiff);
	}

	[ClientRpc]
	public void RpcCreateObject(string mouseButton, Vector3 currMousePos, Vector2 angleDiff) { 
		GameObject spawnObj;
		if (mouseButton == "left") {
			spawnObj = leftObj;
		} else {
			spawnObj = rightObj;
		}
		GameObject newObj = Instantiate (spawnObj, GetPlacePos(leftObj,currMousePos), Quaternion.identity);
		newObj.GetComponent<Spawnable> ().angleDiff = angleDiff;
		Debug.Log (currentPower);
		//currentPower = currentPower - newObj.GetComponent<Spawnable>().cost;
		RpcModEnergy(- newObj.GetComponent<Spawnable>().cost);
		Debug.Log (currentPower);
		toCreateR = 0f;
		Debug.Log (newObj);
		Debug.Log (newObj.GetComponent<Attackable>().netId);
		Debug.Log ("Network Spawning obj");

		//NetworkServer.Spawn (newObj);
		Debug.Log ("After Networkserver spawn");
	}

	[ClientRpc]
	public void RpcModEnergy(float diff) {
		currentPower = currentPower + diff;
	}
	// expects a spawnable object, gets position to be placed, which handles special case of block.
	// block should never be placed on top of P1
	public Vector3 GetPlacePos(GameObject obj, Vector3 mousePos) {
		if (obj.name != "Block") {
			return new Vector3 (mousePos.x, mousePos.y, 0);
		} else {
			Player player = FindObjectOfType<Player> ();
			if (player) {
				GameObject P1 = FindObjectOfType<Player> ().gameObject;
				float Playerx = P1.transform.position.x;
				float Playery = P1.transform.position.y;
				float bufferx = obj.GetComponent<Renderer> ().bounds.size.x / 2 + P1.GetComponent <Renderer> ().bounds.size.x / 2 + 0.1f;
				float buffery = obj.GetComponent<Renderer> ().bounds.size.y / 2 + P1.GetComponent <Renderer> ().bounds.size.y / 2 + 0.1f;
				if (mousePos.x < Playerx - bufferx || mousePos.x > Playerx + bufferx || mousePos.y < Playery - buffery || mousePos.y > Playery + buffery) {
					return new Vector3 (mousePos.x, mousePos.y, 0);
				} else {
					// get a new location to place block
					float leftx = mousePos.x - (Playerx - bufferx);
					float rightx = (Playerx + bufferx) - mousePos.x;
					float topy = (Playery + buffery) - mousePos.y;
					float boty = mousePos.y - (Playery - buffery);
					float finalx;
					float finaly;
					if (leftx < rightx)
						finalx = Playerx - bufferx;
					else
						finalx = Playerx + bufferx;

					if (boty < topy)
						finaly = Playery - buffery;
					else
						finaly = Playery + buffery;

					if (Mathf.Abs (finaly - mousePos.y) < Mathf.Abs (finalx - mousePos.x))
						return new Vector3 (mousePos.x, finaly, 0);
					else
						return new Vector3 (finalx, mousePos.y, 0);
				}
			} else {
				return new Vector3 (mousePos.x, mousePos.y, 0);
			}
		}
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
