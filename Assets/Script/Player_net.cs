﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_net : NetworkBehaviour {

	private GameManager gameManager;
	//private GUIHandler gui;
	//P1
	public string leftKey = "a";
	public string rightKey = "d";
	public string upKey = "w";
	public string downKey = "space";
	public string jumpKey = "w";

	//p2
	public Texture2D defaultTexture; 
	public Texture2D leftFull;
	public Texture2D rightFull;
	public Texture2D noneFull;

	public CursorMode curMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	//	public GameObject bombClass;
	//	public GameObject boxObstacle;
	//	public GameObject fanItem;

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
	public bool singlePlayer = true;

	float deadY = 30.0f;
	float toCreateL = 0f;
	float toCreateR = 0f;
	Vector3 initDownL;
	Vector3 initDownR;
	GameManager gm;
	PlayerCursor pc;
	public bool godPlayer = false;
	// Use this for initialization
	void Start () {
		//string ip = Network.player.ipAddress;
		//GameObject.Find("IPText").GetComponent<Text>().text = ip ;

		if (!isLocalPlayer) {
			Destroy (this);
			return;
		}
		Player[] oldPlayers = GameObject.FindObjectsOfType<Player> ();
		Debug.Log (oldPlayers);
		Movement oldMovement = GetComponent<Movement>();
		foreach (Player pl in oldPlayers) {
			if (pl != GetComponent<Player>()) {
				godPlayer = true;
				oldMovement = pl.gameObject.GetComponent<Movement> ();
			}
		}
		gm = FindObjectOfType<GameManager> ();
		gm.InitGod (true);
		Debug.Log (godPlayer);
		GUIHandler gui = FindObjectOfType<GUIHandler> ();

		if (singlePlayer) {
			pc = GetComponent<PlayerCursor> ();
			//GameObject.FindObjectOfType<CameraFollow> ().setTarget( GetComponent<Movement>() );
			//pc.CmdTurnToGod ();
			FindObjectOfType<GUIHandler> ().GodTarget = pc;
			//gui.HealthTarget = oldMovement.gameObject;
			GameObject.FindObjectOfType<CameraFollow> ().setTarget( GetComponent<Movement> ());
			gui.HealthTarget = gameObject;
			//godPlayer = true;
		}
		else if (godPlayer) {
			pc = GetComponent<PlayerCursor> ();
			GameObject.FindObjectOfType<CameraFollow> ().setTarget( oldMovement );
			pc.CmdTurnToGod ();
			gui.HealthTarget = oldMovement.gameObject;
		} else {
			GameObject.FindObjectOfType<CameraFollow> ().setTarget( GetComponent<Movement> ());
			gui.HealthTarget = gameObject;
		}
	}

	// Update is called once per frame
	void Update () {
		bool lD = Input.GetKeyDown (leftKey);
		bool l = Input.GetKey (leftKey);
		bool rD = Input.GetKeyDown (rightKey);
		bool r = Input.GetKey (rightKey);
		bool aD = Input.GetKey (downKey);
		bool jD = Input.GetKeyDown (jumpKey);
		if (lD) {
			GetComponent<Player> ().updateControls (lD, l, rD, r, aD, jD);
			if (isServer) {
				GetComponent<Player> ().RpcControls (lD, l, rD, r, aD, jD);
			} else {
				GetComponent<Player> ().CmdControls (lD, l, rD, r, aD, jD);
			}
			Debug.Log (netId);
		}
		if (singlePlayer) {
			playerControl ();
		} if (godPlayer) {
			playerControl ();
		} else {
			GetComponent<Player> ().updateControls (lD, l, rD, r, aD, jD);
			if (isServer) {
				GetComponent<Player> ().RpcControls (lD, l, rD, r, aD, jD);
			} else {
				GetComponent<Player> ().CmdControls (lD, l, rD, r, aD, jD);
			}
		}
	}

	void playerControl() { 
		

		checkCursor (pc.currentPower, leftObj.GetComponent<Spawnable> ().cost, rightObj.GetComponent<Spawnable> ().cost);
		if (Input.mouseScrollDelta.y == -1) {
			//Debug.Log ("scroll down");
			//Debug.Log (gm.godButtons.Count);
			int nextInd = gm.currIndex - 1;
			if (nextInd < 0) {
				nextInd = gm.godButtons.Count - 1;
			}
			//Debug.Log (nextInd);
			gm.currIndex = nextInd;
			gm.godButtons [nextInd].setSpawnObj ("left");
		}
		if (Input.mouseScrollDelta.y == 1) {
			//Debug.Log ("scroll up");
			int nextInd = gm.currIndex + 1;
			if (nextInd >= gm.godButtons.Count) {
				nextInd = 0;
			}
			//Debug.Log (nextInd);
			gm.currIndex = nextInd;
			gm.godButtons [nextInd].setSpawnObj ("left");
		}

		if (Input.mousePosition.y < (Screen.height - deadY) || Input.mousePosition.x < deadX) {
			Vector3 currMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			if (Input.GetMouseButtonDown (0)) {
				float cost = pc.leftObj.GetComponent<Spawnable> ().cost;
				if (pc.currentPower >= cost) {
					if (pc.leftObj.GetComponent<Spawnable> ().instantDeploy) {/*
						GameObject newObj = Instantiate (leftObj, GetPlacePos(leftObj,currMousePos), Quaternion.identity);
						newObj.GetComponent<Spawnable> ().angleDiff = Vector2.zero;
						currentPower = currentPower - cost;
						toCreateL = 0f;
						Debug.Log (gm);
						Debug.Log (newObj);
						RpcCreateObject (newObj);*/
						//Debug.Log ("Attempting RPC call");
						pc.CmdCreateObject ("left",currMousePos,Vector2.zero);
					} else {
						initDownL = currMousePos;
						toCreateL = cost;
					}
				} else {
					FindObjectOfType<GUIHandler> ().P2EnergyBarFlashRed ();
				}
			}
			if (Input.GetMouseButtonUp (0) && toCreateL != 0f) {
				pc.CmdCreateObject ("left", new Vector3 (initDownL.x, initDownL.y, 0), new Vector2 (currMousePos.x - initDownL.x, currMousePos.y - initDownL.y));
			}
			if (Input.GetMouseButtonDown (1)) {
				float cost = pc.rightObj.GetComponent<Spawnable> ().cost;
				if (pc.currentPower >= cost) {
					if (pc.rightObj.GetComponent<Spawnable> ().instantDeploy) {
						pc.CmdCreateObject ("right",currMousePos,Vector2.zero);
					} else {
						initDownR = currMousePos;
						toCreateR = cost;
					}
				} else {
					FindObjectOfType<GUIHandler> ().P2EnergyBarFlashRed ();
				}
			}
			if (Input.GetMouseButtonUp (1) && toCreateR != 0f) {
				pc.CmdCreateObject ("right", new Vector3 (initDownR.x, initDownR.y, 0), new Vector2 (currMousePos.x - initDownR.x, currMousePos.y - initDownR.y));
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


	// Use this for initialization
	void initCursor () {

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

}