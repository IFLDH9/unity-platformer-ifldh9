using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
   
    public ClientManager clientManager = null;
    bool spawned = false;

    

    public override void OnStartServer()
    {
        base.OnStartServer();

        //  random = Random.Range(10, 20);
        //  Debug.Log(random + "ennyit generált");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // base.OnServerAddPlayer(conn,playerControllerId);

        //   clientManager = Instantiate(clientManager, new Vector3(0, 0, 0), Quaternion.identity);
        // bool sikerult = NetworkServer.SpawnWithClientAuthority(clientManager.gameObject,conn);

        //   if(sikerult == true)
        {
            //     Debug.Log("sikerult");
            //}else
            //{
            //  Debug.Log("nem sikerult");
            //}

            Debug.Log("csatlakozott valaki");

            GameObject otherPlayer = (GameObject)Instantiate(playerPrefab, new Vector3(34, 200), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, otherPlayer, playerControllerId);
            NetworkServer.SpawnWithClientAuthority(otherPlayer,otherPlayer);


            // if (player == null)
            //{
            //  player = GameObject.Find("Player");
            // NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            //  return;

            //        }

            //  if (player != null)
            //{
            //  GameObject otherPlayer = (GameObject)Instantiate(playerPrefab, new Vector3(34, 200), Quaternion.identity);
            //    NetworkServer.AddPlayerForConnection(conn, otherPlayer, playerControllerId);
            //}
            //player.GetComponent<Player>().color = Color.red;
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        //   RpcUpdateMap(random);
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

