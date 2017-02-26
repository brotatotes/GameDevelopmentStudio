using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Attackable : NetworkBehaviour {

	public float bottomOfTheWorld = -10.0f;
	public float health = 100.0f;
	public float max_health = 100.0f;
	public float energy = 100.0f;
	public float max_energy = 100.0f;
	public bool alive = true;
	public bool immortal = false;
	public string faction = "noFaction";
	public GameObject HitEffect;
	public GameObject HealEffect;
	public float EnergyRegenRate = 10.0f;

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
			RpcDied ();
			//Destroy (gameObject);
		}
		modifyEnergy(EnergyRegenRate * Time.deltaTime);
	}

	[ClientRpc]
	void RpcDied()
	{
		Destroy (gameObject);
	}

	public void damageObj(float damage) {
		if (!isServer)
			return;
		//Debug.Log ("Damage Taken. Health before: " + health);
		/*
		health = Mathf.Max(Mathf.Min(max_health, health - damage),0);
		if (damage > 0) {
			GameObject explosion = GameObject.Instantiate (HitEffect, transform.position, Quaternion.identity);
		} else if (damage < 0) {
			GameObject explosion = GameObject.Instantiate (HealEffect, transform.position, Quaternion.identity);
		}
		//Debug.Log("Health afterwards: " + health);
		if (health < 0) {
			alive = false;
		} else {
			alive = true;
		}*/
		RpcDamage (damage);
	}

	[ClientRpc]
	void RpcDamage(float damage) {
		health = Mathf.Max(Mathf.Min(max_health, health - damage),0);
		if (damage > 0) {
			GameObject explosion = GameObject.Instantiate (HitEffect, transform.position, Quaternion.identity);
		} else if (damage < 0) {
			GameObject explosion = GameObject.Instantiate (HealEffect, transform.position, Quaternion.identity);
		}
		//Debug.Log("Health afterwards: " + health);
		if (health < 0) {
			alive = false;
		} else {
			alive = true;
		}
	}

	public void modifyEnergy(float energyDiff) {
		if (!isServer)
			return;
		//Debug.Log ("Damage Taken. Health before: " + health);
		/*
		energy = Mathf.Max(Mathf.Min(max_energy, energy + energyDiff),0);
		if (energyDiff > 20) {
			GameObject explosion = GameObject.Instantiate (HealEffect, transform.position, Quaternion.identity);
		}*/
		//Debug.Log("Health afterwards: " + health);
		RpcEnergy(energyDiff);
	}

	[ClientRpc]
	public void RpcEnergy(float energyDiff) {
		energy = Mathf.Max(Mathf.Min(max_energy, energy + energyDiff),0);
		if (energyDiff > 20) {
			GameObject healFX = GameObject.Instantiate (HealEffect, transform.position, Quaternion.identity);
		}
	}
		
	public void resetHealth() {
		damageObj (-1000f);
	}

	public void addToVelocity(Vector2 veloc )
	{
		if (!isServer)
			return;
		if (movementController) {
			movementController.addToVelocity(veloc);
		} 
		RpcKnockback (veloc);
	}
	[ClientRpc]
	public void RpcKnockback(Vector2 veloc) {
		if (movementController) {
			movementController.addToVelocity(veloc);
		} 
	}
}
