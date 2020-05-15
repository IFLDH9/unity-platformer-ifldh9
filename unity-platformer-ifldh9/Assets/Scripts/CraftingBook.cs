using System.Collections.Generic;
using UnityEngine;

public class CraftingBook : MonoBehaviour
{
    [SerializeField] private List<CraftRecipe> craftingRecipes = new List<CraftRecipe>();
    Inventory inventory;

    private void Update()
    {
        if (inventory == null)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();

            foreach (Player player in players)
            {
                if (player.isLocalPlayer)
                {
                    inventory = player.GetComponent<Inventory>();
                    break;
                }
            }
        }
    }

    public Item checkForRecipe(Item item1, Item item2, Item item3, Item item4, Item resultItem)
    {
        Item[] items = new Item[5];
        items[0] = item1;
        items[1] = item2;
        items[2] = item3;
        items[3] = item4;
        items[4] = resultItem;

        foreach (CraftRecipe recipe in craftingRecipes)
        {
            if (recipe.isItMatchingRecipe(items))
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
                    inventory.items[44] = newItem;
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
