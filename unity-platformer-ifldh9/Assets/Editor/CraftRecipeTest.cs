using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class CraftRecipeTest
{
    Item item1 = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Wood.asset", typeof(Item));
    Item item2 = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Stone.asset", typeof(Item));
    CraftRecipe craftRecipe = (CraftRecipe)AssetDatabase.LoadAssetAtPath("Assets/Recipes/TorchRecipe.asset", typeof(CraftRecipe));

    [Test]
    public void IsItGoodTest_Good()
    {
        string[] materials = { "wood", "stone", null, null };
        int[] stacks = { 1, 1, 0, 0 };
        int resultStack = 1;

        Item item1 = Object.Instantiate(this.item1);
        Item item2 = Object.Instantiate(item1);
        Item item3 = null;
        Item item4 = null;

        item1.itemName = "wood";
        item2.itemName = "stone";

        Item[] items = { item1, item2, item3, item4 };

        CraftRecipe recipe = Object.Instantiate(craftRecipe);
        recipe.materials = materials;
        recipe.stacks = stacks;
        recipe.resultStack = resultStack;

        bool test = recipe.isItMatchingRecipe(items);
        Assert.That(test, Is.EqualTo(true));
    }
    [Test]
    public void IsItGoodTest_BadItems()
    {
        string[] materials = { "wood", "stone", null, null };
        int[] stacks = { 1, 1, 0, 0 };
        int resultStack = 1;

        Item item1 = Object.Instantiate(this.item1);
        Item item2 = Object.Instantiate(item1);
        Item item3 = null;
        Item item4 = null;

        item1.itemName = "stone";
        item2.itemName = "stone";

        Item[] items = { item1, item2, item3, item4 };

        CraftRecipe recipe = Object.Instantiate(craftRecipe);
        recipe.materials = materials;
        recipe.stacks = stacks;
        recipe.resultStack = resultStack;

        bool test = recipe.isItMatchingRecipe(items);
        Assert.That(test, Is.EqualTo(false));
    }

    [Test]
    public void IsItGoodTest_BadStacks()
    {
        string[] materials = { "wood", "stone", null, null };
        int[] stacks = { 2, 2, 0, 0 };
        int resultStack = 1;

        Item item1 = Object.Instantiate(this.item1);
        Item item2 = Object.Instantiate(item1);
        Item item3 = null;
        Item item4 = null;

        item1.itemName = "stone";
        item2.itemName = "stone";

        Item[] items = { item1, item2, item3, item4 };

        CraftRecipe recipe = Object.Instantiate(craftRecipe);
        recipe.materials = materials;
        recipe.stacks = stacks;
        recipe.resultStack = resultStack;

        bool test = recipe.isItMatchingRecipe(items);
        Assert.That(test, Is.EqualTo(false));
    }

    [Test]
    public void IsItGoodTest_WrongNumberOfItems()
    {
        string[] materials = { "wood", "stone", null, null };
        int[] stacks = { 2, 2, 0, 0 };
        int resultStack = 1;

        Item item1 = Object.Instantiate(this.item1);
        Item item2 = Object.Instantiate(item1);
        Item item3 = null;
        Item item4 = null;

        item1.itemName = "stone";
        item2.itemName = "stone";

        Item[] items = { item2, item3, item4 };

        CraftRecipe recipe = Object.Instantiate(craftRecipe);
        recipe.materials = materials;
        recipe.stacks = stacks;
        recipe.resultStack = resultStack;

        bool test = recipe.isItMatchingRecipe(items);
        Assert.That(test, Is.EqualTo(false));
    }
}