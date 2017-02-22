using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitboxMaker : NetworkBehaviour {

	public GameObject hitboxClass;
	void Start () {}
	void Update() {}
	public void createHitbox(Vector2 hitboxScale, Vector2 offset,float damage, float hitboxDuration, Vector2 knockback,bool fixedKnockback,string faction, bool followObj)  {
		Debug.Log ("normal create Hitbox");
		Debug.Log (netId);
		RpcCreateHitbox (hitboxScale, offset, damage, hitboxDuration, knockback, fixedKnockback, faction, followObj);
	}

	//[ClientRpc]
	void RpcCreateHitbox(Vector2 hitboxScale, Vector2 offset,float damage, float hitboxDuration, Vector2 knockback,bool fixedKnockback,string faction, bool followObj) {
		Debug.Log ("creating Hitbox");

		Vector3 newPos = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);
		GameObject go = Instantiate(hitboxClass,newPos,Quaternion.identity) as GameObject; 
		hitbox newBox = go.GetComponent<hitbox> ();
		newBox.setScale (hitboxScale);
		newBox.setDamage (damage);
		newBox.setHitboxDuration (hitboxDuration);
		newBox.setKnockback (knockback);
		newBox.setFixedKnockback (fixedKnockback);
		newBox.setFaction (faction);
		if (followObj) {
			newBox.setFollow (gameObject,offset);
		}
		//NetworkServer.Spawn (go);
	}
}
