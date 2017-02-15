﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour {

	public float bottomOfTheWorld = -10.0f;
	public float health = 100.0f;
	public float max_health = 100.0f;
	public float energy = 100.0f;
	public bool alive = true;
	public bool immortal = false;
	public string faction = "noFaction";

	Movement movementController;
	// Use this for initialization
	void Start () {
		movementController = gameObject.GetComponent<Movement> ();
		health = Mathf.Min (health, max_health);
	}
	
	// Update is called once per frame
	void Update () {
		alive = transform.position.y >= bottomOfTheWorld && health > 0;
		if (!alive && !immortal) {
			Destroy (gameObject);
		}
	}

	public void damageObj(float damage) {
		//Debug.Log ("Damage Taken. Health before: " + health);
		health = Mathf.Min(max_health, health - damage);
		//Debug.Log("Health afterwards: " + health);
		if (health < 0) {
			alive = false;
		} else {
			alive = true;
		}
	}
	public void resetHealth() {
		damageObj (-1000f);
	}

	public void addToVelocity(Vector2 veloc )
	{
		if (movementController) {
			movementController.addToVelocity(veloc);
		} 
	}
}
