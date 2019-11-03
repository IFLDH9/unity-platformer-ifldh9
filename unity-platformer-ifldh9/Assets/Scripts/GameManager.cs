﻿using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.LWRP;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Map map = null;
    public static GameManager instance = null;
    public Camera cam;
    public EnviromentController enviromentMap;
    public Vector3Int focus;
    public LightController lightController;

    public float timer = 0;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

       // DontDestroyOnLoad(gameObject);
        // player = GetComponent<Transform>();
        map =GetComponent<Map>();
        enviromentMap = GetComponent<EnviromentController>();
        cam = Camera.main;
        lightController = GetComponent<LightController>();

       InitGame();
       RespawnPlayer();
      //  player.GetComponent<Transform>().position = new Vector3Int(0, 198, 0);
    }

    void InitGame()
    {
        map.GenerateMap();
        enviromentMap.CreateEnviroment(map.map);
    }

    
    void Update()
    {
        Transform playerTrans = player.GetComponent<Transform>();
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
            
            Light2D torch = lightController.GetTorch(posInt);
            if (Vector3.Distance(playerPos, pos) < 3.0f && torch != null)
            {
                lightController.RemoveTorch(torch);
                enviromentMap.torchTileMap.SetTile(posInt, null);
            }

           if (focus == posInt)
            {
                if (timer > 0.7f)
                {
                    Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {0}]", pos.x, pos.y));
                    Debug.Log(string.Format("Co-ords of player is [X: {0} Y: {0}]", playerTrans.position.x, playerTrans.position.y));
                    Debug.Log(string.Format("distance {0}", Vector3.Distance(playerPos, pos)));
                    TileBase tile = map.tilemap.GetTile(posInt);
                    Block block = (Block)tile;
                    // Debug.Log(string.Format("tile {0}", tile.name));
                    Debug.Log(string.Format("tile {0}", map.map[posInt.x, posInt.y]));
                    if (map.tilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
                    {
                        //  Debug.Log(string.Format("poz {0}", posInt.x ));
                        block.Drop(posInt);
                        map.map[posInt.x, posInt.y] = 0;
                        map.tilemap.SetTile(posInt, null);
                        map.UpdateMap(map.map, map.tilemap, posInt.x, posInt.y);
                    }
                    //waitingButtonUp = true;
                    timer = 0;
                }

            } else
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
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
            Vector3Int playerPosInt = new Vector3Int((int)playerTrans.position.x,(int)playerTrans.position.y,0);
            if (!map.tilemap.HasTile(posInt) && playerPosInt != posInt &&
                posInt != new Vector3Int(playerPosInt.x,playerPosInt.y+1,0) && HasTileAround(posInt))
            {
                map.map[posInt.x, posInt.y] = 1;
                map.tilemap.SetTile(posInt,map.dirtBlock);
            }
        }

        if(Input.GetMouseButtonDown(2))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
            if (lightController.GetTorch(posInt)== null && Vector3.Distance(playerPos, pos) < 3.0f 
                && map.tilemap.HasTile(posInt) && !enviromentMap.torchTileMap.HasTile(posInt))
            {
                lightController.PutDownTorch(posInt);
                enviromentMap.torchTileMap.SetTile(posInt,enviromentMap.normalTorch);
            }
        }
    }

    public bool HasTileAround(Vector3Int posInt)
    {
        if(map.tilemap.HasTile(new Vector3Int(posInt.x+1,posInt.y,posInt.z)) || map.tilemap.HasTile(new Vector3Int(posInt.x-1, posInt.y, posInt.z)) ||
            map.tilemap.HasTile(new Vector3Int(posInt.x, posInt.y+1, posInt.z)) || map.tilemap.HasTile(new Vector3Int(posInt.x, posInt.y-1, posInt.z)))
        {
            return true;
        }
        return false;
    }

    public void RespawnPlayer()
    {

        int randomX = 0;
        int randomY = 0;
        bool foundASpot = false;

        for(int i = 0; !foundASpot;++i)
        {
            randomX = Random.Range(0, 200);
            randomY = Random.Range(0, map.map.GetUpperBound(1) - 1);
            for (int y = randomY; y < map.map.GetUpperBound(1)-4; y++)
            {
                if(map.tilemap.HasTile(new Vector3Int(randomX,y,0)) && !map.tilemap.HasTile(new Vector3Int(randomX, y+1, 0))
                    )
            {
                    player.GetComponent<Transform>().position=new Vector3Int(randomX, y+1,0);
                    Debug.Log(string.Format("Co-ords [X: {0} Y: {0}]", randomX, y));
                    foundASpot = true;
                    break;
            }
            }

        } 
      }
    }


