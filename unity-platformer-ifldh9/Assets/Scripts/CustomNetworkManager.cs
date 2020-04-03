using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    GameObject player;
    public GameObject clientManager=null;
    bool spawned = false;
    //int random;

    public override void OnStartServer()
    {
        base.OnStartServer();
       
        //  random = Random.Range(10, 20);
        //  Debug.Log(random + "ennyit generált");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if(spawned==false)
        {
        clientManager = Instantiate(clientManager, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(clientManager);
            spawned = true;
        }
      
        Debug.Log("csatlakozott valaki");
        if (player == null)
        {
            player = GameObject.Find("Player");
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            return;

        }
    
        if (player != null)
        {
            GameObject otherPlayer = (GameObject)Instantiate(playerPrefab, new Vector3(34, 200), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, otherPlayer, playerControllerId);
        }
        //player.GetComponent<Player>().color = Color.red;
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        //   RpcUpdateMap(random);
        }

    [ClientRpc]
    public void RpcUpdateMap(int number)
    {
        //   MemoryStream stream = new MemoryStream(map);
        //  object[] args = bf.Deserialize(stream) as object[]; ;
        Debug.Log("ennyit kaptam" + number);
        // foreach (var o in args)
        // {
        //   Debug.Log("arg-class: " + o.GetType() + ": " + o);
        //}
    }
}


    //public void RespawnPlayer()
    //   {
    //       int randomX = 0;
    //       bool foundASpot = false;

    //       for (int i = 0; !foundASpot; ++i)
    //       {
    //           randomX = Random.Range(0, 200);
    //           for (int y = (map.map.GetUpperBound(1)-1); y > 0; y--)
    //           {

    //               if (map.tilemap.HasTile(new Vector3Int(randomX, y, 0)))
    //               {
    //                   player.GetComponent<Transform>().position = new Vector3Int(randomX, y + 1, 0);
    //                   Debug.Log(randomX + "  " + y);
    //                   foundASpot = true;
    //                   break;
    //               }
    //           }
    //       }
    //}

