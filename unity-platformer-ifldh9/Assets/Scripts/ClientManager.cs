using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClientManager : NetworkBehaviour
{
    public GameManager gamemanager;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("localplayer");
        CmdUpdateMap();
    }

    [ClientRpc]
     void RpcUpdateMap(Byte[] map,Boolean final)
    {
        gamemanager.map.byteBuffer.AddRange(map);
        if (final == true)
        {
            gamemanager.map.BytesToMap(gamemanager.map.byteBuffer.ToArray());
            gamemanager.map.RenderFromCompleteMap();



            StringBuilder builder = new StringBuilder();
            foreach (byte number in gamemanager.map.byteBuffer)
            {
                builder.Append(number.ToString());
            }

          Debug.Log("ez lett végül: " + builder.ToString());
        }
       
        
    }

    [Command]
   public void CmdUpdateMap()
    {

        List<Byte> mapBytes = gamemanager.map.MapToByteList();

        StringBuilder builder = new StringBuilder();
        foreach (byte number in mapBytes)
        {
            builder.Append(number.ToString());
        }

        Debug.Log("ezt csinalta : " + builder.ToString());

        Debug.Log(mapBytes);

        int transSize = 1024;
        while(mapBytes.Count > 0)
        {
            Debug.Log("nagysag:" + mapBytes.Count);
            if(mapBytes.Count < 1024)
            {
                RpcUpdateMap(mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
              RpcUpdateMap(mapBytes.GetRange(0,transSize).ToArray(),false);
              mapBytes.RemoveRange(0, transSize);
            }
        }

         //This sends a message from the server to all connected clients that have THIS GameObject and tells them to execute all code inside the "RpcClientChangeColor" function.

        //This executes the "ChangeColor" function on the server ONLY.
        if (isServer)
        {
            Debug.Log("serveren fut");
            Debug.Log("szerver vagyok");

        } else
        {
            Debug.Log("kliensen fut");
        }

    }


    void Awake()
    {
  
        if(isServer)
        {
            Debug.Log("szerver vagyok");
        }

        if (isClient)
        {
            Debug.Log("kliens vagyok");
        }
        gamemanager = GameObject.FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority)
        {
            Debug.Log("van auth");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("localplayer");
        CmdUpdateMap();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }
}
