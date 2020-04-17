using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnviromentController : MonoBehaviour
{

    public List<Block> trees;
    public AnimatedTile normalTorch;
    public AnimatedTile torchOnWall;
    public Tilemap treeTileMap;
    public Tilemap torchTileMap;
    public Block droppedTorch;
    public int[,] treeMap;
    public List<Byte> byteBuffer = new List<Byte>();

    public int columns;
    public int rows;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void GenerateArray(int columns, int rows)
    {
        treeMap = new int[columns, rows];
        for (int x = 0; x < treeMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < treeMap.GetUpperBound(1); y++)
            {
                treeMap[x, y] = 0;
            }
        }
    }


    public void CreateEnviroment(int[,] map,Tilemap tilemap)
    {
        GenerateArray(columns,rows);
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            for (int y = map.GetUpperBound(1)-1; y > 115; y--)
            { 
               if((map[x,y] == 2 || map[x, y] == 3) && UnityEngine.Random.Range(0,100) > 80)
                {
                    Block block =(Block)tilemap.GetTile(new Vector3Int(x,y,0));
                    if(block is DirtBlock && map[x,y+1] == 0 && map[x , y+2] == 0 && map[x , y+3] == 0 && map[x, y+4] == 0)
                    {
                        int random = UnityEngine.Random.Range(1, 3);
                        switch(random)
                        {
                            case 1:
                                treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), trees[0]);
                                treeMap[x, y] = 1;
                                break;
                            case 2:
                                treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), trees[1]);
                                treeMap[x, y] = 2;
                                break;
                            case 3:
                                treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), trees[2]);
                                treeMap[x, y] = 3;
                                break;
                        }
                        x++;
                    }
                }else
                {
                    treeMap[x, y] = 0;
                }
            }
        }
    }

    public void RenderTreeTileMapFromCompleteTreeMap()
    {
        treeTileMap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < treeMap.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < treeMap.GetUpperBound(1); y++)
            {
                switch (treeMap[x,y])
                {
                    case 0:
                        treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), null);
                        break;
                    case 1:
                        treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), trees[0]);
                        break;
                    case 2:
                        treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), trees[1]);
                        break;
                    case 3:
                        treeTileMap.SetTile(new Vector3Int(x, y + 1, 0), trees[2]);
                        break;
                }
            }
        }
    }

    public List<Byte> TreeMapToByteList()
    {
        List<Byte> mapBytes = new List<Byte>();

        for (int x = 0; x < treeMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < treeMap.GetUpperBound(1); y++)
            {
                mapBytes.Add((byte)treeMap[x, y]);
            }
        }
        return mapBytes;
    }


    public void BytesToMap(Byte[] mapBytes)
    {
        if (treeMap == null)
        {
            Debug.Log("a map null");

            GenerateArray(columns, rows);
            treeMap = new int[columns, rows];
        }

        int counter = 0;
        for (int x = 0; x < treeMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < treeMap.GetUpperBound(1); y++)
            {
                treeMap[x, y] = mapBytes[counter];
                counter++;
            }
        }
    }

}
