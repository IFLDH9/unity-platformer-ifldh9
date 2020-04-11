using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public GameManager gameManager;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    // Start is called before the first frame update




    void Start()
    {
        gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        CmdUpdateMap();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
         cinemachineVirtualCamera = UnityEngine.Object.FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = this.transform;
        if (gameManager == null)
        {
            Debug.Log("nincs gamemanager");
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    [ClientRpc]
    void RpcUpdateMap(Byte[] map, Boolean final)
    {
        gameManager.map.byteBuffer.AddRange(map);
        if (final == true)
        {
            gameManager.map.BytesToMap(gameManager.map.byteBuffer.ToArray());
            gameManager.map.RenderFromCompleteMap();



            StringBuilder builder = new StringBuilder();
            foreach (byte number in gameManager.map.byteBuffer)
            {
                builder.Append(number.ToString());
            }

            Debug.Log("ez lett végül: " + builder.ToString());
        }


    }

    [Command]
    public void CmdUpdateMap()
    {

        List<Byte> mapBytes = gameManager.map.MapToByteList();

        StringBuilder builder = new StringBuilder();
        foreach (byte number in mapBytes)
        {
            builder.Append(number.ToString());
        }

        Debug.Log("ezt csinalta : " + builder.ToString());

        Debug.Log(mapBytes);

        int transSize = 1024;
        while (mapBytes.Count > 0)
        {
            Debug.Log("nagysag:" + mapBytes.Count);
            if (mapBytes.Count < 1024)
            {
                RpcUpdateMap(mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
                RpcUpdateMap(mapBytes.GetRange(0, transSize).ToArray(), false);
                mapBytes.RemoveRange(0, transSize);
            }
        }

        //This sends a message from the server to all connected clients that have THIS GameObject and tells them to execute all code inside the "RpcClientChangeColor" function.

        //This executes the "ChangeColor" function on the server ONLY.
        if (isServer)
        {
            Debug.Log("serveren fut");
            Debug.Log("szerver vagyok");

        }
        else
        {
            Debug.Log("kliensen fut");
        }

    }
}
