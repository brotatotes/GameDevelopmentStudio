using UnityEngine;
using System.Collections;

public class hitbox : MonoBehaviour {

	public float damage = 10.0f;
	public Vector2 knockback = new Vector2(0.0f,40.0f);
	public float hitboxDuration = 1.0f;
	public bool timedHitbox = true;

	// Use this for initialization
	void Start () {
		Debug.Log ("explosion created");
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hitboxDuration > 0.0f && timedHitbox) {
			hitboxDuration = hitboxDuration - Time.deltaTime;
		} else {
			GameObject.Destroy (gameObject);
		}
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
		Debug.Log ("Ontriggerenter");
		if (other.gameObject.GetComponent<Controller2D>()) {
			Debug.Log ("correct collision");
			Controller2D otherObj = other.gameObject.GetComponent<Controller2D> ();
			otherObj.damageObj (damage);
			otherObj.Move (knockback);
		}
	}
}
