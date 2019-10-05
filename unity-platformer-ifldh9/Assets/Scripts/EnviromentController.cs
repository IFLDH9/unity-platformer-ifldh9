using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnviromentController : MonoBehaviour
{

    public List<Block> trees;
    public Tilemap tilemap;

    void Start()
    {
       tilemap = GetComponent<Tilemap>();
    }

   
    void Update()
    {
        
    }

    public void createEnviroment(int[,] map)
    {


        tilemap.SetTile(new Vector3Int(0, 0, 0), trees[1]);
        tilemap.SetTile(new Vector3Int(0, 4, 0), trees[1]);
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            for (int y = 0; y < map.GetUpperBound(1)-4; y++)
            { 
               if(map[x,y] == 1 && Random.Range(0,100) > 80)
                {
                    if(map[x,y+1] == 0 && map[x , y+2] == 0 && map[x , y+3] == 0 && map[x, y+4] == 0)
                    {
                        tilemap.SetTile(new Vector3Int(x,y,0),trees[Random.Range(0, trees.Count)]);
                        x++;
                    }
                }
            }
        }
    }

}
