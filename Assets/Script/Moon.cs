using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {
	PlayerCursor pc;

	public float  moonPower = 0.0f;
	ParticleSystem.MainModule moonPart;
	public GameObject tiny_proj;
	public GameObject small_proj;
	public GameObject mid_proj;
	public GameObject large_proj;
	GameObject curPlayer;
	string MoonLevel;
	public float levelGap = 3.0f;
	float mLevelGap = 3.0f;
	float introLevelGap = 3.0f;
	public bool moonActive = true;

	Color black;
	Color red;
	Color white;
	Color blue;
	Color shining;


	// Use this for initialization
	void Start () {
		black = new Color(0f, 0f, 0f, 190f/255f);
		red = new Color(1f, 0f, 0f, 8f/255f);
		//yellow = new Color(1f, 1f, 0f, 8f/255f);
		//green = new Color(0f, 1f, 0.5f, 8f/255f);
		blue = new Color(0f, 0.5f, 1f, 8f/255f);
		shining = new Color(1f, 1f, 1f, 50f/255f);
		white = new Color (0f, 0f, 0f, 8 / 255f);

		//GameObject moonObj = GameObject.FindGameObjectWithTag("Moon");
		//		Debug.Log ("Moon");
		ParticleSystem partsys = GetComponentInChildren<ParticleSystem> ();
		//		Debug.Log ("Moon2");
		mLevelGap = introLevelGap;
		moonPart = partsys.main;
		MoonLevel = "blue";
		pc = FindObjectOfType<PlayerCursor> ();
		curPlayer = FindObjectOfType<Player> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (moonActive) {
			MoonStuff ();
		}
	}

	public void MoonStuff() {
		//moonLevel = moonLevel + Time.deltaTime;
		setMoonLevel (Time.deltaTime);
		if (moonPower < -mLevelGap * 3  && MoonLevel != "black") {
			MoonLevel = "black";
			moonPart.startColor = black;
			pc.rechargeRate = 10.0f;
			mLevelGap = levelGap;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = tiny_proj;
		} else if (moonPower > -mLevelGap * 3 && moonPower < -mLevelGap && MoonLevel != "red") {
			MoonLevel = "red";
			moonPart.startColor = red;
			pc.rechargeRate = 6.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = tiny_proj;
		} else if (moonPower > -mLevelGap &&  moonPower < mLevelGap && MoonLevel != "white") {
			MoonLevel = "white";
			moonPart.startColor = white;
			pc.rechargeRate = 5.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = small_proj;
		} else if (moonPower > mLevelGap &&  moonPower < mLevelGap * 3 && MoonLevel != "blue") {
			MoonLevel = "blue";
			moonPart.startColor = blue;
			pc.rechargeRate = 4.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = mid_proj;
		} else if (moonPower > mLevelGap * 3 &&  MoonLevel != "shining") {
			MoonLevel = "shining";
			moonPart.startColor = shining;
			pc.rechargeRate = 3.0f;
			curPlayer.GetComponent<Fighter> ().canShoot = true;
			curPlayer.GetComponent<Shooter>().projectile = large_proj;
		}
	}
	public void setMoonLevel(float diff) {
		moonPower = Mathf.Max(-mLevelGap * 4,Mathf.Min(mLevelGap * 4,moonPower + diff));
	}
}
