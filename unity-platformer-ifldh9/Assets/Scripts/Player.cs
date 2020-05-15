using Cinemachine;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Player : NetworkBehaviour
{
    public GameObject nameText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    public float timer = 0;
    public Vector3Int focus;
    public Inventory inventory;

    public UnityEvent OnMiningEvent;
    public UnityEvent OnStoppingMiningEvent;

    [SyncVar]
    public string text;

    private void FixedUpdate()
    {
        nameText.GetComponent<TMP_Text>().text = text;
    }

    void Start()
    {
        if (OnMiningEvent == null)
            OnMiningEvent = new UnityEvent();

        if (OnStoppingMiningEvent == null)
            OnStoppingMiningEvent = new UnityEvent();

        gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        inventory = GetComponent<Inventory>();
        if (isClient)
        {
            CmdUpdateMap();
            CmdUpdateEnviroment();
            CmdRespawnPlayer(this.gameObject);
        }
        nameText = Instantiate(nameText, transform.position, Quaternion.identity, transform);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        cinemachineVirtualCamera = UnityEngine.Object.FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = this.transform;
    }

    [Command]
    public void CmdTakeTorchDown(Vector3 pos)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(pos);
        Light2D torch = gameManager.lightController.GetTorch(posInt);

        gameManager.lightController.RemoveTorch(torch);
        gameManager.enviromentMap.torchMatrix[posInt.x, posInt.y - 1] = 0;
        gameManager.enviromentMap.torchTilemap.SetTile(posInt, null);
        gameManager.enviromentMap.droppedTorch.Drop(posInt);
        RpcTakeTorchDown(pos);
    }

    [ClientRpc]
    public void RpcTakeTorchDown(Vector3 pos)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(pos);
        Light2D torch = gameManager.lightController.GetTorch(posInt);

        gameManager.lightController.RemoveTorch(torch);
        gameManager.enviromentMap.torchMatrix[posInt.x, posInt.y - 1] = 0;
        gameManager.enviromentMap.torchTilemap.SetTile(posInt, null);
        gameManager.enviromentMap.droppedTorch.Drop(posInt);
    }

    private void Update()
    {

        if (isLocalPlayer)
        {
            Transform playerTrans = transform;
            if (transform.position.y < -10)
            {
                transform.position = new Vector3(gameManager.map.foregroundTileMatrix.GetUpperBound(0) / 2, 140, 0);
            }
            if (Input.GetMouseButtonUp(0))
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

                if (focus == posInt && !gameManager.enviromentMap.backgroundTilemap.HasTile(new Vector3Int(posInt.x, posInt.y + 1, posInt.z)))
                {
                    AudioHandler audio = GetComponent<AudioHandler>();
                    if (timer > 0.7f)
                    {
                        audio.PlayBreakBlock();
                        CmdHitBlock(playerPos, pos, posInt);
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
            }

            if (Input.GetMouseButtonDown(1))
            {
                Item usedItem = inventory.inHand;
                if (usedItem != null)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 0;
                    Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
                    Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
                    Vector3Int playerPosInt = new Vector3Int((int)playerTrans.position.x, (int)playerTrans.position.y, 0);
                    AudioHandler audio = GetComponent<AudioHandler>();

                    switch (usedItem.blocktype)
                    {
                        case Item.BlockType.FOREGROUND:
                            if (!gameManager.map.foregroundTilemap.HasTile(posInt) && playerPosInt != posInt &&
                       posInt != new Vector3Int(playerPosInt.x, playerPosInt.y + 1, 0) && gameManager.HasTileAround(posInt))
                            {
                                if (usedItem.canBePutDown == true)
                                {
                                    audio.PlayPlaceBlock();
                                    switch (usedItem.itemName)
                                    {
                                        case "Dirt":
                                            CmdPutBlockDown(posInt, 2);
                                            break;

                                        case "Stone":
                                            CmdPutBlockDown(posInt, 3);
                                            break;

                                        case "Wood":
                                            CmdPutBlockDown(posInt, 4);
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
                    && !gameManager.map.foregroundTilemap.HasTile(posInt) && gameManager.enviromentMap.backgroundTilemap.HasTile(posInt) && !gameManager.enviromentMap.torchTilemap.HasTile(posInt))
                            {
                                audio.PlayPlaceBlock();
                                CmdPutTorchDown(posInt);
                                inventory.useItem();
                                inventory.inventoryUI[0].UpdateUI();
                                inventory.inventoryUI[1].UpdateUI();
                            }
                            break;
                        case Item.BlockType.BACKGROUND:
                            if (!gameManager.enviromentMap.backgroundTilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
                            {
                                audio.PlayPlaceBlock();
                                switch (usedItem.itemName)
                                {
                                    case "WoodWall":
                                        CmdPutBackgroundBlockDown(posInt, 4);
                                        break;

                                    case "StoneWall":
                                        CmdPutBackgroundBlockDown(posInt, 5);
                                        break;
                                }
                                gameManager.enviromentMap.backgroundTilemap.SetTile(posInt, usedItem.tile);
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
        gameManager.enviromentMap.torchMatrix[pos.x, pos.y] = 1;
        gameManager.enviromentMap.torchTilemap.SetTile(pos, gameManager.enviromentMap.normalTorch);
        RpcPutTorchDown(posInt);
    }

    [ClientRpc]
    public void RpcPutTorchDown(Vector3 posInt)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        gameManager.lightController.PutDownTorch(pos);
        gameManager.enviromentMap.torchMatrix[pos.x, pos.y] = 1;
        gameManager.enviromentMap.torchTilemap.SetTile(pos, gameManager.enviromentMap.normalTorch);
    }


    [Command]
    public void CmdPutBackgroundBlockDown(Vector3 posInt, int block)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        switch (block)
        {
            case 4:
                gameManager.enviromentMap.backgroundTileMatrix[pos.x, pos.y - 1] = 4;
                gameManager.enviromentMap.backgroundTilemap.SetTile(pos, gameManager.enviromentMap.woodWall);
                break;

            case 5:
                gameManager.enviromentMap.backgroundTileMatrix[pos.x, pos.y - 1] = 5;
                gameManager.enviromentMap.backgroundTilemap.SetTile(pos, gameManager.enviromentMap.stoneWall);
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
                gameManager.enviromentMap.backgroundTileMatrix[pos.x, pos.y - 1] = 4;
                gameManager.enviromentMap.backgroundTilemap.SetTile(pos, gameManager.enviromentMap.woodWall);
                break;

            case 5:
                gameManager.enviromentMap.backgroundTileMatrix[pos.x, pos.y - 1] = 5;
                gameManager.enviromentMap.backgroundTilemap.SetTile(pos, gameManager.enviromentMap.stoneWall);
                break;
        }
    }


    [Command]
    public void CmdPutBlockDown(Vector3 posInt, int block)
    {
        Vector3Int pos = Vector3Int.RoundToInt(posInt);
        switch (block)
        {
            case 2:
                gameManager.map.foregroundTileMatrix[pos.x, pos.y] = 2;
                gameManager.map.foregroundTilemap.SetTile(pos, gameManager.map.dirtBlock);
                break;

            case 3:
                gameManager.map.foregroundTileMatrix[pos.x, pos.y] = 3;
                gameManager.map.foregroundTilemap.SetTile(pos, gameManager.map.stoneBlock);
                break;

            case 4:
                gameManager.map.foregroundTileMatrix[pos.x, pos.y] = 4;
                gameManager.map.foregroundTilemap.SetTile(pos, gameManager.map.woodBlock);
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
                gameManager.map.foregroundTileMatrix[pos.x, pos.y] = 2;
                gameManager.map.foregroundTilemap.SetTile(pos, gameManager.map.dirtBlock);
                break;

            case 3:
                gameManager.map.foregroundTileMatrix[pos.x, pos.y] = 3;
                gameManager.map.foregroundTilemap.SetTile(pos, gameManager.map.stoneBlock);
                break;

            case 4:
                gameManager.map.foregroundTileMatrix[pos.x, pos.y] = 4;
                gameManager.map.foregroundTilemap.SetTile(pos, gameManager.map.woodBlock);
                break;
        }
    }

    [TargetRpc]
    public void TargetUpdateMap(NetworkConnection target, Byte[] map, Boolean final)
    {
        gameManager.map.byteBuffer.AddRange(map);
        if (final == true)
        {
            gameManager.map.BytesToForegroundTileMatrix(gameManager.map.byteBuffer.ToArray());
            gameManager.map.RenderFromCompleteForegroundTileMatrix();
        }
    }

    [Command]
    public void CmdUpdateEnviroment()
    {

        List<Byte> mapBytes = gameManager.enviromentMap.BackgroundTileMatrixToByteList();

        StringBuilder builder = new StringBuilder();
        foreach (byte number in mapBytes)
        {
            builder.Append(number.ToString());
        }

        int transSize = 1024;
        while (mapBytes.Count > 0)
        {
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


        mapBytes = gameManager.enviromentMap.TorchMatrixToByteList();
        while (mapBytes.Count > 0)
        {
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
    public void TargetUpdateTreeEnviroment(NetworkConnection target, Byte[] map, Boolean final)
    {
        gameManager.enviromentMap.byteBuffer.AddRange(map);
        if (final == true)
        {
            gameManager.enviromentMap.BytesToBackgroundTileMatrix(gameManager.enviromentMap.byteBuffer.ToArray());
            gameManager.enviromentMap.RenderBackgroundTilemapFromCompleteBackgroundTileMatrix();
        }
    }

    [TargetRpc]
    public void TargetUpdateTorchEnviroment(NetworkConnection target, Byte[] map, Boolean final)
    {
        gameManager.enviromentMap.byteBuffer.AddRange(map);
        if (final == true)
        {
            gameManager.enviromentMap.BytesToTorchMap(gameManager.enviromentMap.byteBuffer.ToArray());
            gameManager.enviromentMap.RenderTorchTilemapFromCompleteTorchMatrix();

            StringBuilder builder = new StringBuilder();
            foreach (byte number in gameManager.enviromentMap.byteBuffer)
            {
                builder.Append(number.ToString());
            }
        }
    }


    [Command]
    public void CmdUpdateMap()
    {

        List<Byte> mapBytes = gameManager.map.ForegroundTileMatrixToByteList();

        StringBuilder builder = new StringBuilder();
        foreach (byte number in mapBytes)
        {
            builder.Append(number.ToString());
        }

        int transSize = 1024;
        while (mapBytes.Count > 0)
        {
            if (mapBytes.Count < 1024)
            {
                TargetUpdateMap(connectionToClient, mapBytes.GetRange(0, mapBytes.Count).ToArray(), true);
                mapBytes.RemoveRange(0, mapBytes.Count);
            }
            else
            {
                TargetUpdateMap(connectionToClient, mapBytes.GetRange(0, transSize).ToArray(), false);
                mapBytes.RemoveRange(0, transSize);
            }
        }
    }

    [Command]
    public void CmdHitBlock(Vector3 playerPos, Vector3 pos, Vector3 posIntVector3)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(posIntVector3);

        TileBase tile = gameManager.map.foregroundTilemap.GetTile(posInt);
        Block block = (Block)tile;
        TileBase backgroundTile = gameManager.enviromentMap.backgroundTilemap.GetTile(posInt);
        Block backgroundBlock = (Block)backgroundTile;

        if (gameManager.map.foregroundTilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
        {
            gameManager.map.foregroundTileMatrix[posInt.x, posInt.y] = 0;
            gameManager.map.foregroundTilemap.SetTile(posInt, null);
            gameManager.map.UpdateMap(gameManager.map.foregroundTileMatrix, gameManager.map.foregroundTilemap, posInt.x, posInt.y);
            RpcDeleteBlock(posInt);
        }
        else if (gameManager.enviromentMap.backgroundTilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
        {
            gameManager.enviromentMap.backgroundTileMatrix[posInt.x, posInt.y - 1] = 0;
            gameManager.enviromentMap.backgroundTilemap.SetTile(posInt, null);
            RpcDeleteBlock(posInt);
        }
    }

    [ClientRpc]
    public void RpcDeleteBlock(Vector3 posIntVector3)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(posIntVector3);
        TileBase tile = gameManager.map.foregroundTilemap.GetTile(posInt);
        Block block = (Block)tile;
        if (block != null)
        {
            block.Drop(posInt);
            gameManager.map.foregroundTileMatrix[posInt.x, posInt.y] = 0;
            gameManager.map.foregroundTilemap.SetTile(posInt, null);
            gameManager.map.UpdateMap(gameManager.map.foregroundTileMatrix, gameManager.map.foregroundTilemap, posInt.x, posInt.y);
        }

        TileBase backgroundTile = gameManager.enviromentMap.backgroundTilemap.GetTile(posInt);
        Block backgroundBlock = (Block)backgroundTile;

        if (backgroundBlock != null)
        {
            backgroundBlock.Drop(posInt);
            gameManager.enviromentMap.backgroundTileMatrix[posInt.x, posInt.y - 1] = 0;
            gameManager.enviromentMap.backgroundTilemap.SetTile(posInt, null);
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
            for (int y = (gameManager.map.foregroundTileMatrix.GetUpperBound(1) - 1); y > 0; y--)
            {

                if (gameManager.map.foregroundTilemap.HasTile(new Vector3Int(randomX, y, 0)))
                {
                    player.GetComponent<Transform>().position = new Vector3Int(randomX, y + 2, 0);
                    foundASpot = true;

                    break;
                }
            }
        }
        TargetRespawnPlayer(connectionToClient, player.transform.position);
    }

    [TargetRpc]
    public void TargetRespawnPlayer(NetworkConnection target, Vector3 pos)
    {
        this.transform.position = pos;
    }
}
