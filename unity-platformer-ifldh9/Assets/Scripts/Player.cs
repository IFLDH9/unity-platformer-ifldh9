using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Player : NetworkBehaviour
{
    public GameManager gameManager;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float timer = 0;
    public Vector3Int focus;
    // Start is called before the first frame update

    void Start()
    {
        gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        CmdUpdateMap();
        CmdUpdateEnviroment();
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
        if (isLocalPlayer)
        {
            Transform playerTrans = transform;
            if (Input.GetMouseButton(0))
            {
                timer += Time.deltaTime;
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
                Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);

                // Light2D torch = lightController.GetTorch(posInt);
                //  if (Vector3.Distance(playerPos, pos) < 3.0f && torch != null)
                // {
                //   lightController.RemoveTorch(torch);
                //  enviromentMap.torchTileMap.SetTile(posInt, null);
                //  enviromentMap.droppedTorch.Drop(posInt);
                //}

                if (focus == posInt)
                {
                    if (timer > 0.7f)
                    {

                        CmdHitBlock(playerPos, pos, posInt);
                        //waitingButtonUp = true;
                        timer = 0;
                    }

                }
                else
                {
                    focus.Set(posInt.x, posInt.y, posInt.z);
                    timer = 0;
                }
            }
            else
            {
                timer = 0;
                //  waitingButtonUp = false;
            }
        }
    }

    [TargetRpc]
    void TargetUpdateMap(NetworkConnection target, Byte[] map, Boolean final)
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
    public void CmdUpdateEnviroment()
    {

        List<Byte> mapBytes = gameManager.enviromentMap.TreeMapToByteList();

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
                TargetUpdateEnviroment(connectionToClient, mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
                TargetUpdateEnviroment(connectionToClient, mapBytes.GetRange(0, transSize).ToArray(), false);
                mapBytes.RemoveRange(0, transSize);
            }
        }
    }

    [TargetRpc]
    void TargetUpdateEnviroment(NetworkConnection target, Byte[] map, Boolean final)
    {
        gameManager.enviromentMap.byteBuffer.AddRange(map);
        if (final == true)
        {
            gameManager.enviromentMap.BytesToMap(gameManager.enviromentMap.byteBuffer.ToArray());
            gameManager.enviromentMap.RenderTreeTileMapFromCompleteTreeMap();

            StringBuilder builder = new StringBuilder();
            foreach (byte number in gameManager.enviromentMap.byteBuffer)
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
                TargetUpdateMap(connectionToClient, mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
                TargetUpdateMap(connectionToClient,mapBytes.GetRange(0, transSize).ToArray(), false);
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

    [Command]
    public void CmdHitBlock(Vector3 playerPos,Vector3 pos,Vector3 posIntVector3)
    {

        Vector3Int posInt = Vector3Int.RoundToInt(posIntVector3);
           // Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {0}]", pos.x, pos.y));
        //Debug.Log(string.Format("Co-ords of player is [X: {0} Y: {0}]", playerTrans.position.x, playerTrans.position.y));
        //Debug.Log(string.Format("distance {0}", Vector3.Distance(playerPos, pos)));
        TileBase tile = gameManager.map.tilemap.GetTile(posInt);
        Block block = (Block)tile;
        // TileBase backgroundTile = enviromentMap.treeTileMap.GetTile(posInt);
        // Block backgroundBlock = (Block)backgroundTile;

        // Debug.Log(string.Format("tile {0}", tile.name));
        //  Debug.Log(string.Format("tile {0}", map.map[posInt.x, posInt.y]));
        if (gameManager.map.tilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
        {
            //  Debug.Log(string.Format("poz {0}", posInt.x ));
          //  block.Drop(posInt);
            gameManager.map.map[posInt.x, posInt.y] = 0;
            gameManager.map.tilemap.SetTile(posInt, null);
            gameManager.map.UpdateMap(gameManager.map.map, gameManager.map.tilemap, posInt.x, posInt.y);
            RpcDeleteBlock(posInt);
        }

      //  if (enviromentMap.treeTileMap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
        {
            //  Debug.Log(string.Format("poz {0}", posInt.x ));
        //    backgroundBlock.Drop(posInt);
          //  enviromentMap.treeTileMap.SetTile(posInt, null);
        }
    }

    [ClientRpc]
    public void RpcDeleteBlock(Vector3 posIntVector3)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(posIntVector3);
        gameManager.map.map[posInt.x, posInt.y] = 0;
        gameManager.map.tilemap.SetTile(posInt, null);
        gameManager.map.UpdateMap(gameManager.map.map, gameManager.map.tilemap, posInt.x, posInt.y);
    }
}
