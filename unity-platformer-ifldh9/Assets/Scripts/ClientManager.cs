using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientManager : NetworkBehaviour
{
    public GameObject gamemanager;


    [ClientRpc]
     void RpcClientUpdateMap()
    {
        Debug.Log("kliensen fut");
    }

    [Command]
     void UpdateMap()
    {
        RpcClientUpdateMap(); //This sends a message from the server to all connected clients that have THIS GameObject and tells them to execute all code inside the "RpcClientChangeColor" function.

        Debug.Log("serveren fut"); //This executes the "ChangeColor" function on the server ONLY.
    }


    void Start()
    {
  
        gamemanager = GameObject.Find("GameManager");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UpdateMap();
        }
                //This sends a request to the server to execute the "CmdChangeColor" function on the server.
       


    }
}
