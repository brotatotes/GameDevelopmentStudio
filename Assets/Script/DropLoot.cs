using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour {

	public GameObject healthPowerUp;
	public float chanceDropHealth;
	void Start () {}
	void Update () {}

	void OnDestroy () {
		float rand = Random.value;
		if (rand * 100 < chanceDropHealth) {
			GameObject go = Instantiate(healthPowerUp,transform.position,Quaternion.identity) as GameObject; 
			go.GetComponent<Movement> ().addToVelocity (new Vector2 (Random.Range (-15, 15), 30));
		}
	}
}
