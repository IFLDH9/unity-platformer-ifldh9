using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
  
    bool spawned = false;
   public GameObject database;

    public void Awake()
    {
        networkAddress = "84.236.43.35";
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        //  random = Random.Range(10, 20);
        //  Debug.Log(random + "ennyit generált");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
      
        {
            GameObject otherPlayer = (GameObject)Instantiate(playerPrefab, new Vector3(34, 250), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, otherPlayer, playerControllerId);
            NetworkServer.SpawnWithClientAuthority(otherPlayer,otherPlayer);
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }
}
