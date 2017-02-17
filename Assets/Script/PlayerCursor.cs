﻿using UnityEngine;
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
	float toCreateL = 0f;
	float toCreateR = 0f;
	Vector3 initDownL;
	Vector3 initDownR;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);
		deadX = FindObjectOfType<GameManager> ().startX;
	}
	
	// Update is called once per frame
	void Update() {
		if (currentPower < 100.0f) {
			currentPower = Mathf.Min (100.0f, currentPower + (Time.deltaTime * rechargeRate));
		}
		if (Input.mousePosition.y < (Screen.height - deadY) || Input.mousePosition.x < deadX) {
			Vector3 currMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			if (Input.GetMouseButtonDown (0)) {
				float cost = leftObj.GetComponent<Spawnable> ().cost;
				if (currentPower >= cost) {
					if (leftObj.GetComponent<Spawnable> ().instantDeploy) {
						GameObject obj = Instantiate (leftObj, GetPlacePos(leftObj,currMousePos), Quaternion.identity);
						obj.GetComponent<Spawnable> ().angleDiff = Vector2.zero;
						currentPower = currentPower - cost;
						toCreateL = 0f;
					} else {
						initDownL = currMousePos;
						toCreateL = cost;
					}
				}
			}
			if (Input.GetMouseButtonUp (0) && toCreateL != 0f) {
				GameObject obj = Instantiate (leftObj, new Vector3(initDownL.x, initDownL.y,0), Quaternion.identity);
				obj.GetComponent<Spawnable> ().angleDiff = new Vector2 (currMousePos.x - initDownL.x, currMousePos.y - initDownL.y);
				//Instantiate (bombClass, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
				currentPower = currentPower - toCreateL;
				toCreateL = 0f;
			}
			if (Input.GetMouseButtonDown (1)) {
				float cost = rightObj.GetComponent<Spawnable> ().cost;
				if (currentPower >= cost) {
					if (rightObj.GetComponent<Spawnable> ().instantDeploy) {
						GameObject obj = Instantiate (rightObj, GetPlacePos(leftObj,currMousePos), Quaternion.identity);
						obj.GetComponent<Spawnable> ().angleDiff = Vector2.zero;
						currentPower = currentPower - cost;
						toCreateR = 0f;
					} else {
						initDownR = currMousePos;
						toCreateR = cost;
					}
				}
			}
			if (Input.GetMouseButtonUp (1) && toCreateR != 0f) {
				GameObject obj = Instantiate (rightObj, new Vector3(initDownR.x, initDownR.y,0), Quaternion.identity);
				obj.GetComponent<Spawnable> ().angleDiff = new Vector2 (currMousePos.x - initDownR.x, currMousePos.y - initDownR.y);
				//Instantiate (bombClass, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
				currentPower = currentPower - toCreateR;
				toCreateR = 0f;
			}
		}
		/*
		if (Input.GetMouseButtonDown (2)) {
			Debug.Log ("Pressed middle click.");
			Instantiate (fanItem, new Vector3 (currMousePos.x, currMousePos.y, 0), Quaternion.identity);
			currentPower = currentPower - fanCost;
		}*/
	}

	// expects a spawnable object, gets position to be placed, which handles special case of block.
	// block should never be placed on top of P1
	public Vector3 GetPlacePos(GameObject obj, Vector3 mousePos) {
		if (obj.name != "Block") {
			return new Vector3 (mousePos.x, mousePos.y, 0);
		} else {
			GameObject P1 = FindObjectOfType<Player> ().gameObject;
			float Playerx = P1.transform.position.x;
			float Playery = P1.transform.position.y;
			float bufferx = obj.GetComponent<Renderer> ().bounds.size.x/2 + P1.GetComponent <Renderer> ().bounds.size.x/2;
			float buffery = obj.GetComponent<Renderer> ().bounds.size.y/2 + P1.GetComponent <Renderer> ().bounds.size.y/2;
			if (mousePos.x < Playerx - bufferx || mousePos.x > Playerx + bufferx || mousePos.y < Playery - buffery || mousePos.y > Playery + buffery) {
				return new Vector3 (mousePos.x, mousePos.y, 0);
			} else {
				// get a new location to place block
				float leftx = mousePos.x - (Playerx-bufferx);
				float rightx = (Playerx + bufferx) - mousePos.x;
				float topy = (Playery + buffery) - mousePos.y;
				float boty = mousePos.y - (Playery-buffery);
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
		}
	}

	void OnMouseEnter(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}

	void OnMouseExit(){
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);	
	}
}
