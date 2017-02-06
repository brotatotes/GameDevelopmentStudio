﻿using UnityEngine;
using System.Collections;

public class disappearing : MonoBehaviour {

	public float duration = 3.0f;
	// Use this for initialization
	void Start () {}

	// Update is called once per frame
	void Update () {
		if (duration > 0.0f) {
			duration = duration - Time.deltaTime;
		} else {
			GameObject.Destroy (gameObject);
		}
	}
	void setDuration (float time) {
		duration = time;
	}
}