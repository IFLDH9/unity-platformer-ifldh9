using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Player : NetworkBehaviour
{
    public GameObject nameText;
    public GameManager gameManager;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float timer = 0;
    public Vector3Int focus;
    Inventory inventory;

    public UnityEvent OnMiningEvent;
    public UnityEvent OnStoppingMiningEvent;

    void Start()
    {
        if (OnMiningEvent == null)
            OnMiningEvent = new UnityEvent();

        if (OnStoppingMiningEvent == null)
            OnStoppingMiningEvent = new UnityEvent();

        gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        inventory = GetComponent<Inventory>();
        CmdUpdateMap();
        CmdUpdateEnviroment();
        CmdRespawnPlayer(this.gameObject);
        nameText = Instantiate(nameText, transform.position, Quaternion.identity, transform);
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

    public void blackTile()
    {
        Vector3Int playerPos = Vector3Int.RoundToInt(transform.position);

        if (gameManager.map.tilemap.HasTile(new Vector3Int(playerPos.x, playerPos.y-1, playerPos.z)))
        {
            gameManager.map.tilemap.SetColor(new Vector3Int(playerPos.x, playerPos.y - 1, playerPos.z),new Color(0,0,0));
        }

    }

    [Command]
    public void CmdTakeTorchDown(Vector3 pos)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(pos);
        Light2D torch = gameManager.lightController.GetTorch(posInt);

        gameManager.lightController.RemoveTorch(torch);
        gameManager.enviromentMap.torchMap[posInt.x, posInt.y - 1] = 0;
        gameManager.enviromentMap.torchTileMap.SetTile(posInt, null);
        gameManager.enviromentMap.droppedTorch.Drop(posInt);
        RpcTakeTorchDown(pos);
    }

    [ClientRpc]
    public void RpcTakeTorchDown(Vector3 pos)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(pos);
        Light2D torch = gameManager.lightController.GetTorch(posInt);

        gameManager.lightController.RemoveTorch(torch);
        gameManager.enviromentMap.torchMap[posInt.x, posInt.y - 1] = 0;
        gameManager.enviromentMap.torchTileMap.SetTile(posInt, null);
        gameManager.enviromentMap.droppedTorch.Drop(posInt);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            Transform playerTrans = transform;
            if(transform.position.y < -10)
            {
                transform.position = new Vector3(gameManager.map.map.GetUpperBound(0)/2,140,0);
            }
            if(Input.GetMouseButtonUp(0))
            {
                OnStoppingMiningEvent.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                OnMiningEvent.Invoke();
                timer += Time.deltaTime;
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
                Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);

                 Light2D torch = gameManager.lightController.GetTorch(posInt);
                  if (Vector3.Distance(playerPos, pos) < 3.0f && torch != null)
                {

                    CmdTakeTorchDown(pos);
                }
                
                if (focus == posInt && !gameManager.enviromentMap.treeTileMap.HasTile(new Vector3Int(posInt.x,posInt.y+1,posInt.z)))
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

            if (Input.GetMouseButtonDown(1))
            {
                Item usedItem = inventory.inHand;
                if (usedItem != null)
                {
                    Debug.Log(usedItem.itemName);
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 0;
                    Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
                    Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
                    Vector3Int playerPosInt = new Vector3Int((int)playerTrans.position.x, (int)playerTrans.position.y, 0);

                    switch (usedItem.blocktype)
                    {
                        case Item.BlockType.FOREGROUND:
                            if (!gameManager.map.tilemap.HasTile(posInt) && playerPosInt != posInt &&
                       posInt != new Vector3Int(playerPosInt.x, playerPosInt.y + 1, 0) && gameManager.HasTileAround(posInt))
                            {
                                if (usedItem.canBePutDown == true)
                                {
                                    switch(usedItem.itemName)
                                    {
                                        case "Dirt":
                                            CmdPutBlockDown(posInt, 2); //2
                                            break;

                                        case "Stone":
                                            CmdPutBlockDown(posInt, 3);//3
                                            break;

                                        case "Wood":
                                            CmdPutBlockDown(posInt, 4);//4
                                            break;
                                    }
                                    inventory.useItem();
                                    inventory.inventoryUI[0].UpdateUI();
                                    inventory.inventoryUI[1].UpdateUI();
                                }
                            }
                            break;
                            case Item.BlockType.TORCH:
                                if (gameManager.lightController.GetTorch(posInt) == null && Vector3.Distance(playerPos, pos) < 3.0f
                        && !gameManager.map.tilemap.HasTile(posInt) && gameManager.enviromentMap.treeTileMap.HasTile(posInt) && !gameManager.enviromentMap.torchTileMap.HasTile(posInt))
                                {
                                    CmdPutTorchDown(posInt);
                                    inventory.useItem();
                                    inventory.inventoryUI[0].UpdateUI();
                                    inventory.inventoryUI[1].UpdateUI();
                                }
                                break;
                        case Item.BlockType.BACKGROUND:
                            if (!gameManager.enviromentMap.treeTileMap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
                            {
                                switch (usedItem.itemName)
                                {
                                    case "WoodWall":
                                        CmdPutBackgroundBlockDown(posInt, 4);
                                        break;

                                    case "StoneWall":
                                        CmdPutBackgroundBlockDown(posInt, 5);//5
                                        break;
                                }
                                gameManager.enviromentMap.treeTileMap.SetTile(posInt, usedItem.tile);
                                inventory.useItem();
                                inventory.inventoryUI[0].UpdateUI();
                                inventory.inventoryUI[1].UpdateUI();
                            }

                            break;
                    }
                }
            }
        }
    }

    [Command]
    public void CmdPutTorchDown(Vector3 posInt)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        gameManager.lightController.PutDownTorch(pos);
        gameManager.enviromentMap.torchMap[pos.x, pos.y] = 1;
        gameManager.enviromentMap.torchTileMap.SetTile(pos, gameManager.enviromentMap.normalTorch);
        RpcPutTorchDown(posInt);
    }

    [ClientRpc]
    public void RpcPutTorchDown(Vector3 posInt)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        gameManager.lightController.PutDownTorch(pos);
        gameManager.enviromentMap.torchMap[pos.x, pos.y] = 1;
        gameManager.enviromentMap.torchTileMap.SetTile(pos, gameManager.enviromentMap.normalTorch);
    }


    [Command]
    public void CmdPutBackgroundBlockDown(Vector3 posInt, int block)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        switch (block)
        {
            case 4:
                gameManager.enviromentMap.treeMap[pos.x, pos.y-1] = 4;
                gameManager.enviromentMap.treeTileMap.SetTile(pos, gameManager.enviromentMap.woodWall);
                break;

            case 5:
                gameManager.enviromentMap.treeMap[pos.x, pos.y-1] = 5;
                gameManager.enviromentMap.treeTileMap.SetTile(pos, gameManager.enviromentMap.stoneWall);
                break;
        }
        RpcPutBackgroundBlockDown(posInt, block);
    }

    [ClientRpc]
    public void RpcPutBackgroundBlockDown(Vector3 posInt, int block)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        switch (block)
        {
            case 4:
                gameManager.enviromentMap.treeMap[pos.x, pos.y-1] = 4;
                gameManager.enviromentMap.treeTileMap.SetTile(pos, gameManager.enviromentMap.woodWall);
                break;

            case 5:
                gameManager.enviromentMap.treeMap[pos.x, pos.y-1] = 5;
                gameManager.enviromentMap.treeTileMap.SetTile(pos, gameManager.enviromentMap.stoneWall);
                break;
        }
    }


    [Command]
    public void CmdPutBlockDown(Vector3 posInt,int block)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        switch (block)
        {
            case 2:
                gameManager.map.map[pos.x, pos.y] = 2;
                gameManager.map.tilemap.SetTile(pos, gameManager.map.dirtBlock);
                //2
                break;

            case 3:
                gameManager.map.map[pos.x, pos.y] = 3;
                gameManager.map.tilemap.SetTile(pos, gameManager.map.stoneBlock);
                //3
                break;

            case 4:
                gameManager.map.map[pos.x, pos.y] = 4;
                gameManager.map.tilemap.SetTile(pos, gameManager.map.woodBlock);
                //4
                break;
        }
        RpcPutBlockDown(posInt, block);
    }

    [ClientRpc]
    public void RpcPutBlockDown(Vector3 posInt, int block)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        switch (block)
        {
            case 2:
                gameManager.map.map[pos.x, pos.y] = 2;
                gameManager.map.tilemap.SetTile(pos, gameManager.map.dirtBlock);
                //2
                break;

            case 3:
                gameManager.map.map[pos.x, pos.y] = 3;
                gameManager.map.tilemap.SetTile(pos, gameManager.map.stoneBlock);
                //3
                break;

            case 4:
                gameManager.map.map[pos.x, pos.y] = 4;
                gameManager.map.tilemap.SetTile(pos, gameManager.map.woodBlock);
                //4
                break;
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
                TargetUpdateTreeEnviroment(connectionToClient, mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
                TargetUpdateTreeEnviroment(connectionToClient, mapBytes.GetRange(0, transSize).ToArray(), false);
                mapBytes.RemoveRange(0, transSize);
            }
        }


        mapBytes = gameManager.enviromentMap.TorchMapToByteList();
        while (mapBytes.Count > 0)
        {
            Debug.Log("nagysag:" + mapBytes.Count);
            if (mapBytes.Count < 1024)
            {
                TargetUpdateTorchEnviroment(connectionToClient, mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
                TargetUpdateTorchEnviroment(connectionToClient, mapBytes.GetRange(0, transSize).ToArray(), false);
                mapBytes.RemoveRange(0, transSize);
            }
        }
    }

    [TargetRpc]
    void TargetUpdateTreeEnviroment(NetworkConnection target, Byte[] map, Boolean final)
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

    [TargetRpc]
    void TargetUpdateTorchEnviroment(NetworkConnection target, Byte[] map, Boolean final)
    {
        gameManager.enviromentMap.byteBuffer.AddRange(map);
        if (final == true)
        {
            gameManager.enviromentMap.BytesToTorchMap(gameManager.enviromentMap.byteBuffer.ToArray());
            gameManager.enviromentMap.RenderTorchTileMapFromCompleteTorchMap();

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
         TileBase backgroundTile = gameManager.enviromentMap.treeTileMap.GetTile(posInt);
         Block backgroundBlock = (Block)backgroundTile;

        // Debug.Log(string.Format("tile {0}", tile.name));
        //  Debug.Log(string.Format("tile {0}", map.map[posInt.x, posInt.y]));
        if (gameManager.map.tilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
        {
            //  Debug.Log(string.Format("poz {0}", posInt.x ));
            // block.Drop(posInt);
            gameManager.map.map[posInt.x, posInt.y] = 0;
            gameManager.map.tilemap.SetTile(posInt, null);
            gameManager.map.UpdateMap(gameManager.map.map, gameManager.map.tilemap, posInt.x, posInt.y);
            Debug.Log("nemfa");
            RpcDeleteBlock(posInt);
        }else if(gameManager.enviromentMap.treeTileMap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
            {
            gameManager.enviromentMap.treeMap[posInt.x, posInt.y-1] = 0;
            gameManager.enviromentMap.treeTileMap.SetTile(posInt, null);
            RpcDeleteBlock(posInt);
        }
    }

    [ClientRpc]
    public void RpcDeleteBlock(Vector3 posIntVector3)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(posIntVector3);
        TileBase tile = gameManager.map.tilemap.GetTile(posInt);
        Block block = (Block)tile;
        if(block != null)
        {
            block.Drop(posInt);
            gameManager.map.map[posInt.x, posInt.y] = 0;
            gameManager.map.tilemap.SetTile(posInt, null);
            gameManager.map.UpdateMap(gameManager.map.map, gameManager.map.tilemap, posInt.x, posInt.y);
        }
       
        TileBase backgroundTile = gameManager.enviromentMap.treeTileMap.GetTile(posInt);
        Block backgroundBlock = (Block)backgroundTile;

        if (backgroundBlock != null)
        {
            backgroundBlock.Drop(posInt);
            gameManager.enviromentMap.treeMap[posInt.x, posInt.y-1] = 0;
            gameManager.enviromentMap.treeTileMap.SetTile(posInt, null);
        }
    }
    [Command]
    public void CmdRespawnPlayer(GameObject player)
    {
        int randomX = 0;
        bool foundASpot = false;

        for (int i = 0; !foundASpot; ++i)
        {
            randomX = UnityEngine.Random.Range(0, 200);
            for (int y = (gameManager.map.map.GetUpperBound(1) - 1); y > 0; y--)
            {

                if (gameManager.map.tilemap.HasTile(new Vector3Int(randomX, y, 0)))
                {
                    player.GetComponent<Transform>().position = new Vector3Int(randomX, y + 2, 0);
                    Debug.Log(randomX + "  " + y);
                    foundASpot = true;
                   
                    break;
                }
            }
        }
        TargetRespawnPlayer(connectionToClient,player.transform.position);
    }


    [TargetRpc]
  public  void TargetRespawnPlayer(NetworkConnection target, Vector3 pos)
    {
        this.transform.position = pos;
    }
}
