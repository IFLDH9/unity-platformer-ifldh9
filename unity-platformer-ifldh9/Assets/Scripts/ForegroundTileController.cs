using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class ForegroundTileController : NetworkBehaviour
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;
    public DirtBlock dirtBlock;
    public StoneBlock stoneBlock;
    public Wood woodBlock;
    [SerializeField] private Transform mapHolder;
    public Tilemap foregroundTilemap;
    public int[,] foregroundTileMatrix;
    public List<Byte> byteBuffer = new List<Byte>();
    public void Awake()
    {
        TilemapRenderer tilemapRenderer = foregroundTilemap.GetComponent<TilemapRenderer>();
        tilemapRenderer.chunkSize = new Vector3Int(64, 64, 64);
    }

    public void RenderFromCompleteForegroundTileMatrix()
    {
        foregroundTilemap.ClearAllTiles();
        for (int x = 0; x < foregroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < foregroundTileMatrix.GetUpperBound(1); y++)
            {
                if (foregroundTileMatrix[x, y] == 2)
                {
                    foregroundTilemap.SetTile(new Vector3Int(x, y, 0), dirtBlock);
                }
                else if (foregroundTileMatrix[x, y] == 3)
                {
                    foregroundTilemap.SetTile(new Vector3Int(x, y, 0), stoneBlock);
                }
                else if (foregroundTileMatrix[x, y] == 4)
                {
                    foregroundTilemap.SetTile(new Vector3Int(x, y, 0), woodBlock);
                }
            }
        }
    }

    private void RenderNewMap(int[,] map, Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    if (y > map.GetUpperBound(1) / 2)
                    {
                        if (Random.Range(1, 101) > 90)
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), stoneBlock);
                            map[x, y] = 3;
                        }
                        else
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), dirtBlock);
                            map[x, y] = 2;
                        }
                    }
                    else if (y > map.GetUpperBound(1) / 3)
                    {
                        if (Random.Range(1, 101) > 70)
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), stoneBlock);
                            map[x, y] = 3;
                        }
                        else
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), dirtBlock);
                            map[x, y] = 2;
                        }


                    }
                    else if (y > map.GetUpperBound(1) / 5)
                    {
                        if (Random.Range(1, 101) > 50)
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), stoneBlock);
                            map[x, y] = 3;
                        }
                        else
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), dirtBlock);
                            map[x, y] = 2;
                        }
                    }
                    else
                    {

                        tilemap.SetTile(new Vector3Int(x, y, 0), stoneBlock);
                        map[x, y] = 3;


                    }
                }
            }
        }
    }

    public void GenerateMap()
    {
        GenerateBasicStructure();
        RenderNewMap(foregroundTileMatrix, foregroundTilemap);
    }

    private void GenerateBasicStructure()
    {
        foregroundTileMatrix = GenerateArray(columns, rows, true);
        float seedTop = Random.Range(80000.0f, 90000.0f);

        int random = Random.Range(7, 13);
        for (int x = 0; x < random; x++)
        {
            foregroundTileMatrix = RandomWalkTopSmoothed(foregroundTileMatrix, Random.Range(10000.0f, 90000.0f), 2, Random.Range(120, 140), Random.Range(140, 160), foregroundTileMatrix.GetUpperBound(0) / random * x, foregroundTileMatrix.GetUpperBound(0) / random * (x + 1));
        }

        int seedCave;
        for (int x = 0; x < Random.Range(3, 12); x++)
        {
            seedCave = Random.Range(1, 900000);
            foregroundTileMatrix = RandomWalkCave(foregroundTileMatrix, seedCave, 1);
        }

        int numberOfCaves = Random.Range(1, 7);
        for (int x = 0; x < numberOfCaves; x++)
        {
            seedCave = Random.Range(1, 900000);
            foregroundTileMatrix = DirectionalTunnel(foregroundTileMatrix, seedCave, 2, Random.Range(3, 8), Random.Range(5, 10), Random.Range(1, 10), Random.Range(5, 30));
        }
    }

    private int[,] GenerateArray(int columns, int rows, bool empty)
    {
        int[,] map = new int[columns, rows];

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1) / 8 * 4; y++)
            {
                map[x, y] = 1;
            }
        }

        for (int x = 1; x < map.GetUpperBound(0); x++)
        {
            for (int y = map.GetUpperBound(1) / 8 * 4; y < map.GetUpperBound(1) - 5; y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public void UpdateMap(int[,] map, Tilemap tilemap, int posX, int posY)
    {

        if (posX == 0 || posX == 1)
        {
            posX = 2;
        }
        else if (posX > rows - 3)
        {
            posX = rows - 3;
        }

        if (posY == 0 || posY == 1)
        {
            posY = 2;
        }
        else
        if (posY < columns - 3)
        {
            posY = columns - 3;
        }

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth, int heightMin, int heightMax, int start, int stop)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int lastHeight = Random.Range(heightMin, heightMax);

        int nextMove = 0;
        int sectionWidth = 0;

        for (int x = start; x <= stop; x++)
        {
            nextMove = rand.Next(Random.Range(2, 5));

            if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
            {
                lastHeight -= Random.Range(1, 3);
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
            {
                lastHeight += Random.Range(1, 3);
                sectionWidth = 0;
            }
            sectionWidth++;

            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }

        return map;
    }

    public static int[,] RandomWalkCave(int[,] map, float seed, int requiredFloorPercent)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int floorX = Random.Range(1, map.GetUpperBound(0) - 1);
        int floorY = Random.Range(1, map.GetUpperBound(1) - 1);
        int reqFloorAmount = ((map.GetUpperBound(1) * map.GetUpperBound(0)) * requiredFloorPercent) / 100;
        int floorCount = 0;

        map[floorX, floorY] = 0;
        floorCount++;
        while (floorCount < reqFloorAmount)
        {
            int randDir = rand.Next(4);

            switch (randDir)
            {
                case 0:
                    if ((floorY + 1) < map.GetUpperBound(1) - 1)
                    {
                        floorY++;

                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 1:
                    if ((floorY - 1) > 1)
                    {
                        floorY--;
                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 2:
                    if ((floorX + 1) < map.GetUpperBound(0) - 1)
                    {
                        floorX++;
                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
                case 3:
                    if ((floorX - 1) > 1)
                    {
                        floorX--;
                        if (map[floorX, floorY] == 1)
                        {
                            map[floorX, floorY] = 0;
                            floorCount++;
                        }
                    }
                    break;
            }
        }
        return map;
    }

    public static int[,] DirectionalTunnel(int[,] map, int seed, int minPathWidth, int maxPathWidth, int maxPathChange, int roughness, int curvyness)
    {
        int tunnelWidth = 1;

        int x = Random.Range(15, map.GetUpperBound(0)-15);

        System.Random rand = new System.Random(seed.GetHashCode());

        for (int i = -tunnelWidth; i <= tunnelWidth; i++)
        {
            map[x + i, 0] = 0;
        }
        for (int y = Random.Range(0, map.GetUpperBound(1) / 2); y < map.GetUpperBound(1); y++)
        {
            if (rand.Next(0, 100) > roughness)
            {
                int widthChange = Random.Range(-maxPathWidth, maxPathWidth);
                tunnelWidth += widthChange;
                if (tunnelWidth < minPathWidth)
                {
                    tunnelWidth = minPathWidth;
                }
                if (tunnelWidth > maxPathWidth)
                {
                    tunnelWidth = maxPathWidth;
                }
            }

            if (rand.Next(0, 100) > curvyness)
            {
                int xChange = Random.Range(-maxPathChange, maxPathChange);
                x += xChange;
                if (x < maxPathWidth)
                {
                    x = maxPathWidth;
                }
                if (x > (map.GetUpperBound(0) - maxPathWidth))
                {
                    x = map.GetUpperBound(0) - maxPathWidth;
                }
            }
            for (int i = -tunnelWidth; i <= tunnelWidth; i++)
            {
                map[x + i, y] = 0;
            }
        }
        return map;
    }

    public List<Byte> ForegroundTileMatrixToByteList()
    {
        List<Byte> mapBytes = new List<Byte>();

        for (int x = 0; x < foregroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < foregroundTileMatrix.GetUpperBound(1); y++)
            {
                mapBytes.Add((byte)foregroundTileMatrix[x, y]);
            }
        }
        return mapBytes;
    }

    public void BytesToForegroundTileMatrix(Byte[] mapBytes)
    {
        if (foregroundTileMatrix == null)
        {
            GenerateArray(columns, rows, false);
            foregroundTileMatrix = new int[columns, rows];
        }

        int counter = 0;
        for (int x = 0; x < foregroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < foregroundTileMatrix.GetUpperBound(1); y++)
            {
                foregroundTileMatrix[x, y] = (int)mapBytes[counter];
                counter++;
            }
        }
    }
}
