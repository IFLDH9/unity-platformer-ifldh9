using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnviromentControllerTest
{
    [Test]
    public void BytesToTreeMapTest()
    {
        BackgroundTileController enviromentController = new GameObject().AddComponent<BackgroundTileController>();
        enviromentController.backgroundTileMatrix = new int[3, 3];

        List<Byte> test = new List<Byte>();
        test.Add(1);
        test.Add(2);
        test.Add(3);
        test.Add(4);

        enviromentController.BytesToBackgroundTileMatrix(test.ToArray());

        string testString = test[0].ToString() + test[1].ToString() + test[2].ToString() + test[3].ToString();

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(enviromentController.backgroundTileMatrix[0, 0].ToString());
        stringBuilder.Append(enviromentController.backgroundTileMatrix[0, 1].ToString());
        stringBuilder.Append(enviromentController.backgroundTileMatrix[1, 0].ToString());
        stringBuilder.Append(enviromentController.backgroundTileMatrix[1, 1].ToString());

        string mapString = stringBuilder.ToString();
        Assert.AreEqual(testString, mapString);
    }

    [Test]
    public void TreeMapToByteListTest()
    {
        var enviromentController = new GameObject().AddComponent<BackgroundTileController>();
        enviromentController.backgroundTileMatrix = new int[3, 3];

        enviromentController.backgroundTileMatrix[0, 0] = 1;
        enviromentController.backgroundTileMatrix[0, 1] = 2;
        enviromentController.backgroundTileMatrix[1, 0] = 3;
        enviromentController.backgroundTileMatrix[1, 1] = 4;

        List<Byte> test = new List<Byte>();
        test.Add((Byte)1);
        test.Add((Byte)2);
        test.Add((Byte)3);
        test.Add((Byte)4);

        List<Byte> list = enviromentController.BackgroundTileMatrixToByteList();

        Assert.AreEqual(test, list);
    }

    [Test]
    public void BytesToTorchMapTest()
    {
        BackgroundTileController enviromentController = new GameObject().AddComponent<BackgroundTileController>();
        enviromentController.torchMatrix = new int[3, 3];

        List<Byte> test = new List<Byte>();
        test.Add(1);
        test.Add(2);
        test.Add(3);
        test.Add(4);

        enviromentController.BytesToTorchMap(test.ToArray());

        string testString = test[0].ToString() + test[1].ToString() + test[2].ToString() + test[3].ToString();

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(enviromentController.torchMatrix[0, 0].ToString());
        stringBuilder.Append(enviromentController.torchMatrix[0, 1].ToString());
        stringBuilder.Append(enviromentController.torchMatrix[1, 0].ToString());
        stringBuilder.Append(enviromentController.torchMatrix[1, 1].ToString());

        string mapString = stringBuilder.ToString();
        Assert.AreEqual(testString, mapString);
    }

    [Test]
    public void TorchMapToByteListTest()
    {
        var enviromentController = new GameObject().AddComponent<BackgroundTileController>();
        enviromentController.torchMatrix = new int[3, 3];

        enviromentController.torchMatrix[0, 0] = 1;
        enviromentController.torchMatrix[0, 1] = 2;
        enviromentController.torchMatrix[1, 0] = 3;
        enviromentController.torchMatrix[1, 1] = 4;

        List<Byte> test = new List<Byte>();
        test.Add((Byte)1);
        test.Add((Byte)2);
        test.Add((Byte)3);
        test.Add((Byte)4);

        List<Byte> list = enviromentController.TorchMatrixToByteList();

        Assert.AreEqual(test, list);
    }
}
