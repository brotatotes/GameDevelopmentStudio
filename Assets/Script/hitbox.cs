using UnityEngine;
using System.Collections;

public class hitbox : MonoBehaviour {

	public float damage = 10.0f;
	public bool fixedKnockback = false;
	public Vector2 knockback = new Vector2(0.0f,40.0f);
	public float hitboxDuration = 1.0f;
	public bool timedHitbox = true;

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
	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Controller2D>()) {
			Controller2D otherObj = other.gameObject.GetComponent<Controller2D> ();
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
				Debug.Log ("KB: " + force);
				otherObj.addToVelocity (new Vector2 (-forceX, -forceY));
			}
		}
	}
}
