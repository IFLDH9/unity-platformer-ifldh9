using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //player.GetComponent<Player>().color = Color.red;
        //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
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
}
