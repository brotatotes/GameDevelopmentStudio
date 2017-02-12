using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {

	public GameObject hitboxClass;
	public float damage = 10.0f;
	public Vector2 knockback = new Vector2(0.0f,40.0f);
	public float fuseDuration = 3.0f;
	public Vector2 hitboxScale = new Vector2 (1.0f, 1.0f);
	public bool timedbomb = true;
	public float hitboxDuration = 0.5f;


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (fuseDuration > 0.0f && timedbomb) {
			fuseDuration = fuseDuration - Time.deltaTime;
		} else {
			GameObject go = Instantiate(hitboxClass,transform.position,Quaternion.identity) as GameObject; 
			hitbox newBox = go.GetComponent<hitbox> ();
			newBox.setScale (hitboxScale);
			newBox.setDamage (damage);
			newBox.setHitboxDuration (hitboxDuration);
			newBox.setKnockback (knockback);

			GameObject.Destroy (gameObject);
		}
	}
	void setDamage(float dmg) {
		damage = dmg;
	}
	void setFuseDuration (float time) {
		fuseDuration = time;
	}
}
