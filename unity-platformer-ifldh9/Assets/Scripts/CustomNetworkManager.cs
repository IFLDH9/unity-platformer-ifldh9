using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{

    [SerializeField] private bool spawned = false;
    [SerializeField] private GameObject database;

    private void Awake()
    {
        networkAddress = "ifldh9.servegame.com";
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        {
            GameObject otherPlayer = (GameObject)Instantiate(playerPrefab, new Vector3(34, 250), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, otherPlayer, playerControllerId);
            NetworkServer.SpawnWithClientAuthority(otherPlayer, otherPlayer);
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }
}
