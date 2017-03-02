using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour {

	public Texture2D defaultTexture; 
	public Texture2D leftFull;
	public Texture2D rightFull;
	public Texture2D noneFull;
	public int divscale = 4;

	public CursorMode curMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public float currentPower = 100.0f;
	public float rechargeRate = 3.0f;
	public bool notEnoughEnergy = false;

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
	public float  moonPower = 0.0f;
	ParticleSystem.MainModule Moon;
	string MoonLevel;
	public float levelGap = 3.0f;
	float mLevelGap = 3.0f;
	float introLevelGap = 3.0f;

	Color black;
	Color red;
	Color orange;
	Color yellow;
	Color green;
	Color blue;
	Color white;

	public GameObject tiny_proj;
	public GameObject small_proj;
	public GameObject mid_proj;
	public GameObject large_proj;
	GameObject curPlayer;

	// Use this for initialization
	void Start () {
//		int w = defaultTexture.width;
//		int h = defaultTexture.height;
//
//		foreach (Texture2D t in new Texture2D[4] {defaultTexture, leftFull, rightFull, noneFull}) {
//			t.Resize (w / divscale, h / divscale);
//			t.Apply ();
//		}
		black = new Color(0f, 0f, 0f, 190f/255f);
		red = new Color(1f, 0f, 0f, 8f/255f);
		yellow = new Color(1f, 1f, 0f, 8f/255f);
		green = new Color(0f, 1f, 0.5f, 8f/255f);
		blue = new Color(0f, 0.5f, 1f, 8f/255f);
		white = new Color(1f, 1f, 1f, 50f/255f);

		GameObject moonObj = GameObject.FindGameObjectWithTag("Moon");
//		Debug.Log ("Moon");
		ParticleSystem moonpart = moonObj.GetComponentInChildren<ParticleSystem> ();
//		Debug.Log ("Moon2");
		mLevelGap = introLevelGap;
		Debug.Log (moonpart.main);
		Moon = moonpart.main;
		Cursor.SetCursor(defaultTexture, hotSpot, curMode);
		deadX = FindObjectOfType<GameManager> ().startX;
		gm = FindObjectOfType<GameManager> ();
		MoonLevel = "blue";
		curPlayer = FindObjectOfType<Player> ().gameObject;
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

	public void MoonStuff() {
		//moonLevel = moonLevel + Time.deltaTime;
		setMoonLevel (Time.deltaTime);
		if (moonPower >= mLevelGap * 5  && MoonLevel != "black") {
			MoonLevel = "black";
			Moon.startColor = black;
			rechargeRate = 10.0f;
			mLevelGap = levelGap;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = tiny_proj;
		} else if (moonPower > mLevelGap * 4 && moonPower < mLevelGap * 5 && MoonLevel != "red") {
			MoonLevel = "red";
			Moon.startColor = red;
			rechargeRate = 6.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = tiny_proj;
		} else if (moonPower > mLevelGap * 3 &&  moonPower < mLevelGap * 4 && MoonLevel != "orange") {
			MoonLevel = "orange";
			Moon.startColor = orange;
			rechargeRate = 5.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = small_proj;
		} else if (moonPower > mLevelGap * 2 &&  moonPower < mLevelGap * 3 && MoonLevel != "yellow") {
			MoonLevel = "yellow";
			Moon.startColor = yellow;
			rechargeRate = 4.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = small_proj;
		} else if (moonPower > mLevelGap &&  moonPower < mLevelGap * 2 && MoonLevel != "green") {
			MoonLevel = "green";
			Moon.startColor = green;
			rechargeRate = 3.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = mid_proj;
		} else if (moonPower < mLevelGap && moonPower > 0 && MoonLevel != "blue") {
			MoonLevel = "blue";
			Moon.startColor = blue;
			rechargeRate = 2.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = mid_proj;
		} else if (moonPower < 0 && MoonLevel != "whites") {
			MoonLevel = "white";
			Moon.startColor = white;
			rechargeRate = 1.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = large_proj;
		}
	}
	public void setMoonLevel(float diff) {
		moonPower = Mathf.Max(-mLevelGap * 2,Mathf.Min(mLevelGap * 5,moonPower + diff));
	}
	// Update is called once per frame
	void Update() {
		MoonStuff ();
		if (currentPower < 100.0f) {
			currentPower = Mathf.Min (100.0f, currentPower + (Time.deltaTime * rechargeRate));
		}
		checkCursor (currentPower, leftObj.GetComponent<Spawnable> ().cost, rightObj.GetComponent<Spawnable> ().cost);
		if (Input.mouseScrollDelta.y == -1) {
			Debug.Log ("scroll down");
			Debug.Log (gm.godButtons.Count);
			int nextInd = gm.currIndex - 1;
			if (nextInd < 0) {
				nextInd = gm.godButtons.Count - 1;
			}
			Debug.Log (nextInd);
			gm.currIndex = nextInd;
			gm.godButtons [nextInd].setSpawnObj ("left");
		}
		if (Input.mouseScrollDelta.y == 1) {
			Debug.Log ("scroll up");
			int nextInd = gm.currIndex + 1;
			if (nextInd >= gm.godButtons.Count) {
				nextInd = 0;
			}
			Debug.Log (nextInd);
			gm.currIndex = nextInd;
			gm.godButtons [nextInd].setSpawnObj ("left");
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
				} else {
					FindObjectOfType<GUIHandler> ().P2EnergyBarFlashRed ();
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
				} else {
					FindObjectOfType<GUIHandler> ().P2EnergyBarFlashRed ();
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
			float bufferx = obj.GetComponent<Renderer> ().bounds.size.x/2 + P1.GetComponent <Renderer> ().bounds.size.x/2 + 0.1f;
			float buffery = obj.GetComponent<Renderer> ().bounds.size.y/2 + P1.GetComponent <Renderer> ().bounds.size.y/2 + 0.1f;
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
