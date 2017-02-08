using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fan : MonoBehaviour {
	public Vector2 knockback = new Vector2(-100.0f,0.0f);
	public GameObject hitboxClass;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject go = Instantiate(hitboxClass,transform.position,Quaternion.identity) as GameObject; 
		hitbox newBox = go.GetComponent<hitbox> ();
		newBox.setScale (new Vector2 (-14.0f, 4.0f));
		newBox.setDamage (0.0f);
		newBox.setHitboxDuration (0.5f);

		GameObject.Destroy (gameObject);
	}
}
