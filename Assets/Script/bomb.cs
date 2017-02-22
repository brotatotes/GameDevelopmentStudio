﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (HitboxMaker))]
public class bomb : MonoBehaviour {
	public float damage = 10.0f;
	public Vector2 knockback = new Vector2(0.0f,40.0f);
	public float fuseDuration = 3.0f;
	public Vector2 hitboxScale = new Vector2 (1.0f, 1.0f);
	public bool timedbomb = true;
	public float hitboxDuration = 0.5f;
	public GameObject ExplosionPrefab;

	// Use this for initialization
	void Start () {}

	// Update is called once per frame
	void Update () {}

	void OnDestroy () {
		GameObject explosion = GameObject.Instantiate (ExplosionPrefab, transform.position, Quaternion.identity);
		explosion.transform.localScale = new Vector3 (hitboxScale.x/16f,hitboxScale.y/16f,hitboxScale.x/16f);
		Debug.Log ("trying to create hitbox");
		Debug.Log (gameObject.GetComponent<HitboxMaker> ());
		gameObject.GetComponent<HitboxMaker> ().createHitbox(hitboxScale,Vector2.zero,damage,hitboxDuration,knockback,false,"noFaction",false);
	}
}
