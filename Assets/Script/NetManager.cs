using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkBehaviour {

	/*
	// called when a client connects 
	public virtual void OnServerConnect(NetworkConnection conn);

	// called when a client disconnects
	public virtual void OnServerDisconnect(NetworkConnection conn)
	{
		NetworkServer.DestroyPlayersForConnection(conn);
	}

	// called when a client is ready
	public virtual void OnServerReady(NetworkConnection conn)
	{
		NetworkServer.SetClientReady(conn);
	}

	// called when a new player is added for a client
	public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	// called when a player is removed for a client
	public virtual void OnServerRemovePlayer(NetworkConnection conn, short playerControllerId)
	{
		PlayerController player;
		if (conn.GetPlayer(playerControllerId, out player))
		{
			if (player.NetworkIdentity != null && player.NetworkIdentity.gameObject != null)
				NetworkServer.Destroy(player.NetworkIdentity.gameObject);
		}
	}

	// called when a network error occurs
	public virtual void OnServerError(NetworkConnection conn, int errorCode);
	{
	}
	// called when connected to a server
	public virtual void OnClientConnect(NetworkConnection conn)
	{
		ClientScene.Ready(conn);
		ClientScene.AddPlayer(0);
	}

	// called when disconnected from a server
	public virtual void OnClientDisconnect(NetworkConnection conn)
	{
		StopClient();
	}

	// called when a network error occurs
	public virtual void OnClientError(NetworkConnection conn, int errorCode);

	// called when told to be not-ready by a server
	public virtual void OnClientNotReady(NetworkConnection conn);
	*/
	/*
	public override void OnClientConnect(NetworkConnection conn) {
		ClientScene.
	}*/
}