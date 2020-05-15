using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class CraftRecipe : ScriptableObject
{
    public string[] materials = new string[4];
    public int[] stacks = new int[4];
    public Item result;
    public int resultStack;

    public bool isItMatchingRecipe(Item[] items)
    {

        for (int i = 0; i < 4; i++)
        {
            switch (stacks[i])
            {
                case 0:
                    break;
                default:
                    if (items[i] == null)
                    {
                        return false;
                    }
                    if (!materials[i].Equals(items[i].itemName) || stacks[i] > items[i].stack)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

}
