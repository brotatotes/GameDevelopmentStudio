﻿using UnityEngine;
using System.Collections.Generic;

public class hitbox : MonoBehaviour {

	List<Movement> collidedObjs = new List<Movement> (); 
	public float damage = 10.0f;
	public bool fixedKnockback = false;
	public Vector2 knockback = new Vector2(0.0f,40.0f);
	public float hitboxDuration = 1.0f;
	public bool timedHitbox = true;
	public string faction = "noFaction";
	public GameObject followObj;
	public bool toFollow = false;
	public Vector2 followOffset;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (hitboxDuration > 0.0f && timedHitbox) {
			hitboxDuration = hitboxDuration - Time.deltaTime;
		} else {
			GameObject.Destroy (gameObject);
		}
		if (toFollow) {
			transform.position = new Vector3(followObj.transform.position.x + followOffset.x, followObj.transform.position.y + followOffset.y,0);
		}
	}
	public void setKnockback(Vector2 kb) {
		knockback = kb;
	}

	public void setFixedKnockback(bool fixedKB) {
		fixedKnockback = fixedKB;
	}

	public void setDamage(float dmg) {
		damage = dmg;
	}
	public void setHitboxDuration (float time) {
		hitboxDuration = time;
	}
	public void setScale(Vector2 scale) {
		transform.localScale = scale;
	}
	public void setFaction(string fact) {
		faction = fact;
	}
	public void setFollow(GameObject obj, Vector2 offset) {
		toFollow = true;
		followObj = obj;
		followOffset = offset;
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Movement>() &&
			!collidedObjs.Contains (other.gameObject.GetComponent<Movement> ())) {
//			Debug.Log ("Detected Collision");
			Movement otherObj = other.gameObject.GetComponent<Movement> ();
			if (faction == "noFaction" || otherObj.faction == "noFaction" ||
			    faction != otherObj.faction) {
				otherObj.damageObj (damage);
				if (fixedKnockback) {
					otherObj.addToVelocity (knockback);
				} else {
					Vector3 otherPos = other.gameObject.transform.position;
					float angle = Mathf.Atan2 (transform.position.y - otherPos.y, transform.position.x - otherPos.x); //*180.0f / Mathf.PI;
					float magnitude = knockback.magnitude;
					float forceX = Mathf.Cos (angle) * magnitude;
					float forceY = Mathf.Sin (angle) * magnitude;
					Vector2 force = new Vector2 (-forceX, -forceY);
					float counterF = (other.gameObject.GetComponent<Movement> ().velocity.y * (1 / Time.deltaTime));
//					Debug.Log ("KB: " + force);
//					Debug.Log (counterF);
					if (counterF < 0) {
						force.y = force.y - counterF;
					}
//					Debug.Log ("KBA: " + force);
					otherObj.addToVelocity (force);
				}
			}
			collidedObjs.Add (other.gameObject.GetComponent<Movement> ());
		}
	}
}
