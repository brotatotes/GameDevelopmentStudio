using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndZone : MonoBehaviour {

	public Text WinMessage;

	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Player>()) {
			WinMessage.text = "You Win!";
			//System.Threading.Thread.Sleep (5000);
			other.gameObject.GetComponent<Player> ().Reset();
		}
		Debug.Log ("trigger");   
	}
}
