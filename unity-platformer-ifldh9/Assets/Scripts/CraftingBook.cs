using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBook : MonoBehaviour
{
    public List<CraftRecipe> craftingRecipes = new List<CraftRecipe>();
    public static CraftingBook instance;

    public void Awake()
    {
        instance = this;
    }

    public Item checkForRecipe(Item item1, Item item2, Item item3, Item item4, Item resultItem)
    {
        //  Debug.Log(item1.itemName + " " + item1.stack);
        // Debug.Log(item2.itemName + " " + item2.stack);
        // Debug.Log(item3.itemName + " " + item3.stack);
        // Debug.Log(item4.itemName + " " + item4.stack);

        Item[] items = new Item[5];
        items[0] = item1;
        items[1] = item2;
        items[2] = item3;
        items[3] = item4;
        items[4] = resultItem;

        foreach (CraftRecipe recipe in craftingRecipes)
        {
            if (recipe.isItGood(items))
            {
                Item newItem = Instantiate(recipe.result);
                newItem.stack = recipe.resultStack;

                if (resultItem == null)
                {
                    for (int i = 0; i < items.Length - 1; i++)
                    {
                        if (items[i] != null)
                        {
                            items[i].stack -= recipe.stacks[i];
                        }
                    }
                    Inventory.instance.items[44] = newItem;
                }
                else if (newItem.name.Equals(resultItem.name))
                {
                    for (int i = 0; i < items.Length - 1; i++)
                    {
                        if (items[i] != null)
                        {
                            items[i].stack -= recipe.stacks[i];
                        }
                    }
                    resultItem.stack += newItem.stack;
                }
                return newItem;
            }
        }
        return null;
    }
}
