using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class disappearing : NetworkBehaviour {

	public float duration = 3.0f;
	public bool toDisappear = true;
	// Use this for initialization
	void Start () {}

	// Update is called once per frame
	void Update () {
		if (duration > 0.0f || !toDisappear) {
			duration = duration - Time.deltaTime;
		} else {
			GameObject.Destroy (gameObject);
			//RpcDied ();
		}
	}
	void setDuration (float time) {
		duration = time;
	}
	[ClientRpc]
	void RpcDied()
	{
		Destroy (gameObject);
	}
}
