using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MapTest
{
    [Test]
    public void MapToBytesTest()
    {
        var map = new GameObject().AddComponent<ForegroundTileController>();
        map.foregroundTileMatrix = new int[3, 3];

        for (int x = 0; x < map.foregroundTileMatrix.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.foregroundTileMatrix.GetUpperBound(1); y++)
            {
                map.foregroundTileMatrix[x, y] = 0;
            }
        }

        map.foregroundTileMatrix[0, 0] = 1;
        map.foregroundTileMatrix[0, 1] = 2;
        map.foregroundTileMatrix[1, 0] = 3;
        map.foregroundTileMatrix[1, 1] = 4;

        List<Byte> test = new List<Byte>();
        test.Add((Byte)1);
        test.Add((Byte)2);
        test.Add((Byte)3);
        test.Add((Byte)4);

        List<Byte> list = map.ForegroundTileMatrixToByteList();

        Assert.AreEqual(test, list);
    }

    [Test]
    public void BytesToMapTest()
    {
        ForegroundTileController map = new GameObject().AddComponent<ForegroundTileController>();
        map.foregroundTileMatrix = new int[3, 3];

        List<Byte> test = new List<Byte>();
        test.Add(1);
        test.Add(2);
        test.Add(3);
        test.Add(4);

        map.BytesToForegroundTileMatrix(test.ToArray());

        string testString = test[0].ToString() + test[1].ToString() + test[2].ToString() + test[3].ToString();

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(map.foregroundTileMatrix[0, 0].ToString());
        stringBuilder.Append(map.foregroundTileMatrix[0, 1].ToString());
        stringBuilder.Append(map.foregroundTileMatrix[1, 0].ToString());
        stringBuilder.Append(map.foregroundTileMatrix[1, 1].ToString());

        string mapString = stringBuilder.ToString();
        Assert.AreEqual(testString, mapString);
    }

}
