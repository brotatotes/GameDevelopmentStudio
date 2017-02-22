using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (HitboxMaker))]
[RequireComponent (typeof (Movement))]
public class Fighter : MonoBehaviour {
	public float damage = 10.0f;
	public Vector2 knockback = new Vector2(0.0f,40.0f);
	public Vector2 hitboxScale = new Vector2 (1.0f, 1.0f);
	public bool timedbomb = true;
	public float hitboxDuration = 0.5f;
	public float attackCooldown = 1.0f;
	public Vector2 offset = new Vector2(0f,0f);

	float currentCooldown = 0.0f;
	string myFac;
	// Use this for initialization
	void Start () {
		myFac = gameObject.GetComponent<Attackable> ().faction;
		currentCooldown = attackCooldown / 2f;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentCooldown > 0.0f) {
			currentCooldown = currentCooldown - Time.deltaTime;
		}
	}

	public bool tryAttack() {
		if (currentCooldown <= 0.0f) {
			currentCooldown = attackCooldown;
			Vector2 realKB = knockback;
			Vector2 realOff = offset;
			if (gameObject.GetComponent<Movement> ().facingLeft) {
				realKB = new Vector2 (-knockback.x, knockback.y);
				realOff = new Vector2 (-offset.x, offset.y);
			}
			gameObject.GetComponent<HitboxMaker> ().createHitbox(hitboxScale,realOff,damage,hitboxDuration,realKB,true,myFac,true);
			return true;
		}
		return false;
	}
}
