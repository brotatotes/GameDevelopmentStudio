using UnityEngine;
using System.Collections;

public class activateGeyser : MonoBehaviour {

	public string collideThisPlayer = "Player 1";
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
	internal void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Geyser");
		if (other.gameObject.GetComponent<Player>().playerName == "Player1") {
			gameObject.GetComponent<Collider2D>().isTrigger = true;
		}
		else {
			gameObject.GetComponent<Collider2D>().isTrigger = false;
		}

		// ActivateGeyser();
	}

}
