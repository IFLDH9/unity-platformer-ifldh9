using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTileController : MonoBehaviour
{

    [SerializeField] private List<Block> trees;
    public AnimatedTile normalTorch;
    [SerializeField] private AnimatedTile torchOnWall;
    public Tilemap backgroundTilemap;
    public Tilemap torchTilemap;
    public Block droppedTorch;
    public int[,] backgroundTileMatrix;
    public int[,] torchMatrix;
    public List<Byte> byteBuffer = new List<Byte>();
    public Block woodWall;
    public Block stoneWall;
    [SerializeField] private LightController lightController;

    [SerializeField] private int columns;
    [SerializeField] private int rows;

    private void Start()
    {
        lightController = GetComponent<LightController>();
    }

    private void GenerateArray(int columns, int rows)
    {
        backgroundTileMatrix = new int[columns, rows];
        torchMatrix = new int[columns, rows];
        for (int x = 0; x < backgroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < backgroundTileMatrix.GetUpperBound(1); y++)
            {
                backgroundTileMatrix[x, y] = 0;
                torchMatrix[x, y] = 0;
            }
        }
    }

    public void CreateEnviroment(int[,] map, Tilemap tilemap)
    {
        GenerateArray(columns, rows);
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = map.GetUpperBound(1) - 1; y > map.GetUpperBound(1) / 2 - 4; y--)
            {
                if (map[x, y] == 2 && UnityEngine.Random.Range(0, 100) > 80 &&
                    map[x, y + 1] == 0 && map[x, y + 2] == 0 && map[x, y + 3] == 0 && map[x, y + 4] == 0)
                {
                    int random = UnityEngine.Random.Range(1, 4);
                    switch (random)
                    {
                        case 1:
                            backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), trees[0]);
                            backgroundTileMatrix[x, y] = 1;
                            break;
                        case 2:
                            backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), trees[1]);
                            backgroundTileMatrix[x, y] = 2;
                            break;
                        case 3:
                            backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), trees[2]);
                            backgroundTileMatrix[x, y] = 3;
                            break;
                    }
                    break;

                }
                else if (map[x, y] != 0)
                {
                    backgroundTileMatrix[x, y] = 0;
                    break;

                }
                else
                {
                    backgroundTileMatrix[x, y] = 0;
                }
            }
        }
    }

    public void RenderBackgroundTilemapFromCompleteBackgroundTileMatrix()
    {
        backgroundTilemap.ClearAllTiles();
        for (int x = 0; x < backgroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < backgroundTileMatrix.GetUpperBound(1); y++)
            {
                switch (backgroundTileMatrix[x, y])
                {
                    case 0:
                        backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), null);
                        break;
                    case 1:
                        backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), trees[0]);
                        break;
                    case 2:
                        backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), trees[1]);
                        break;
                    case 3:
                        backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), trees[2]);
                        break;
                    case 4:
                        backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), woodWall);
                        break;
                    case 5:
                        backgroundTilemap.SetTile(new Vector3Int(x, y + 1, 0), stoneWall);
                        break;
                }
            }
        }
        byteBuffer.Clear();
    }

    public void RenderTorchTilemapFromCompleteTorchMatrix()
    {
        torchTilemap.ClearAllTiles();
        for (int x = 0; x < torchMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < torchMatrix.GetUpperBound(1); y++)
            {
                switch (torchMatrix[x, y])
                {
                    case 0:
                        torchTilemap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    case 1:
                        torchTilemap.SetTile(new Vector3Int(x, y, 0), normalTorch);
                        lightController.PutDownTorch(new Vector3Int(x, y, 0));
                        break;
                    case 2:
                        torchTilemap.SetTile(new Vector3Int(x, y, 0), torchOnWall);
                        break;
                }
            }
        }
    }

    public List<Byte> BackgroundTileMatrixToByteList()
    {
        List<Byte> mapBytes = new List<Byte>();

        for (int x = 0; x < backgroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < backgroundTileMatrix.GetUpperBound(1); y++)
            {
                mapBytes.Add((byte)backgroundTileMatrix[x, y]);
            }
        }
        return mapBytes;
    }

    public void BytesToBackgroundTileMatrix(Byte[] mapBytes)
    {
        if (backgroundTileMatrix == null)
        {
            GenerateArray(columns, rows);
            backgroundTileMatrix = new int[columns, rows];
        }

        int counter = 0;
        for (int x = 0; x < backgroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < backgroundTileMatrix.GetUpperBound(1); y++)
            {
                backgroundTileMatrix[x, y] = mapBytes[counter];
                counter++;
            }
        }
    }

    public List<Byte> TorchMatrixToByteList()
    {
        List<Byte> mapBytes = new List<Byte>();

        for (int x = 0; x < torchMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < torchMatrix.GetUpperBound(1); y++)
            {
                mapBytes.Add((byte)torchMatrix[x, y]);
            }
        }
        return mapBytes;
    }

    public void BytesToTorchMap(Byte[] mapBytes)
    {
        if (torchMatrix == null)
        {
            GenerateArray(columns, rows);
            torchMatrix = new int[columns, rows];
        }

        int counter = 0;
        for (int x = 0; x < torchMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < torchMatrix.GetUpperBound(1); y++)
            {
                torchMatrix[x, y] = mapBytes[counter];
                counter++;
            }
        }
    }
}
