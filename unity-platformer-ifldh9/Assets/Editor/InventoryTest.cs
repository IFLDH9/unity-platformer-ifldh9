using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

public class InventoryTest
{
    Item item1 = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Wood.asset", typeof(Item));

    [Test]
    public void AddTest_GoodItem()
    {
        Item item1 = Object.Instantiate(this.item1);

        Inventory inventory = new Inventory();
        bool test = inventory.Add(item1);

        Assert.IsTrue(test);
    }

    [Test]
    public void AddTest_NoItem()
    {
        Inventory inventory = new Inventory();
        bool test = inventory.Add(null);

        Assert.IsFalse(test);
    }

    [Test]
    public void GetItemTest()
    {
        Item item1 = Object.Instantiate(this.item1);
        Inventory inventory = new Inventory();
        bool test = inventory.Add(item1);
        Item otherItem = inventory.GetItem(item1);

        Assert.AreEqual(otherItem, item1);
    }

    [Test]
    public void UseItemTest_HasMoreStacks()
    {
        Item item1 = Object.Instantiate(this.item1);
        item1.stack = 3;
        Inventory inventory = new Inventory();
        inventory.Add(item1);

        inventory.inHand = inventory.items[0];
        inventory.useItem();

        Assert.AreEqual(inventory.items[0].stack, 2);
    }

    [Test]
    public void UseItemTest_HasNoMoreStacks()
    {
        Item item1 = Object.Instantiate(this.item1);
        item1.stack = 1;
        Inventory inventory = new Inventory();
        inventory.Add(item1);

        inventory.inHand = inventory.items[0];
        inventory.useItem();

        Assert.AreEqual(inventory.items[0], null);
    }

    [Test]
    public void GetNextFreeSpaceTest()
    {
        Item item1 = Object.Instantiate(this.item1);
        item1.stack = 1;
        Inventory inventory = new Inventory();
        inventory.Add(item1);

        int space = inventory.getNextFreeSpace();
        Assert.AreEqual(1, space);
    }

    [Test]
    public void SortTest()
    {
        Item item1 = Object.Instantiate(this.item1);
        Item item2 = Object.Instantiate(this.item1);
        Item item3 = Object.Instantiate(this.item1);

        item1.stack = 1;
        item2.stack = 1;
        item3.stack = 1;
        Inventory inventory = new Inventory();
        inventory.items[0] = item1;
        inventory.items[1] = item2;
        inventory.items[2] = item3;

        inventory.Sort();

        Assert.AreEqual(inventory.items[0].stack, 3);
        Assert.IsNull(inventory.items[1]);
    }
}
