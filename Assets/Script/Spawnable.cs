﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour {

	public string myName;
	public GameObject prefab;
	public float cost;
	public Vector2 angleDiff = Vector2.zero;
	public bool instantDeploy = true;
	// Use this for initialization
	void Start () {
		GameObject soundfx = FindObjectOfType<GameManager> ().soundfx.gameObject;
		if (prefab.name.Contains ("Block")) {
			soundfx.transform.FindChild ("PlaceBlock").GetComponent<AudioSource> ().Play ();
		} else if (prefab.name.Contains("Enemy")) {
			soundfx.transform.FindChild ("EnemyGrunt").GetComponent<AudioSource> ().Play ();
		} else if (prefab.name.Contains("Giant")) {
			soundfx.transform.FindChild ("GiantGrunt").GetComponent<AudioSource> ().Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
