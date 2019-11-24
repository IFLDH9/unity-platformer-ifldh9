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

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void CreateEnviroment(int[,] map,Tilemap tilemap)
    {
        treeTileMap.SetTile(new Vector3Int(0, 0, 0), trees[1]);
        treeTileMap.SetTile(new Vector3Int(0, 4, 0), trees[1]);
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            for (int y = map.GetUpperBound(1)-1; y > 115; y--)
            { 
               if(map[x,y] == 1 && Random.Range(0,100) > 80)
                {
                    Block block =(Block)tilemap.GetTile(new Vector3Int(x,y,0));
                    if(block is DirtBlock && map[x,y+1] == 0 && map[x , y+2] == 0 && map[x , y+3] == 0 && map[x, y+4] == 0)
                    {
                        treeTileMap.SetTile(new Vector3Int(x,y+1,0),trees[Random.Range(0, trees.Count)]);
                        x++;
                    }
                }
            }
        }
    }



}
