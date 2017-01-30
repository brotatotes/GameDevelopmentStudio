using UnityEngine;
using System.Collections;

public class deathZone : MonoBehaviour {

	internal void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("trigger");
	}
}
