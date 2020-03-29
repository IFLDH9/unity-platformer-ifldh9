using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{

    public static Inventory instance;
    public Item[] items = new Item[40];
    public static Item inHand = null;
    public static int select = 0;
    public InventoryUI[] inventoryUI;

    public Item Torch;
    CraftingBook craftingBook;
    public void Awake()
    {
        instance = this;
        Item newItem = Instantiate(Torch);
        Add(newItem);
        Debug.Log("inventory :)");
        Debug.Log(select);
        Debug.Log("added torch :)");
        //  Debug.Log(inHand.itemName);
        Debug.Log(select);
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public void Start()
    {
        craftingBook = CraftingBook.instance;
    }

    public bool Add(Item item)
    {
        if (item.stack > 0)
        {
            Debug.Log(item.itemName + "has been added");
            Item listItem = GetItem(item);
            if (listItem != null)
            {
                Debug.Log(listItem.name + " " + listItem.stack.ToString());
                listItem.stack += item.stack;
                return true;
            }
            else
            {
                int place = getNextFreeSpace();
                if (place > -1)
                {
                    items[place] = item;
                    inHand = items[select];
                    if (onItemChangedCallback != null)
                    {
                        onItemChangedCallback.Invoke();
                    }
                    return true;
                }
                return false;
            }
        }
        else
        {
            item = null;
            return false;
        }

    }

    public Item GetItem(Item item)
    {
        foreach (Item listItem in items)
        {
            if (listItem != null && listItem.itemName.Equals(item.itemName))
            {
                return listItem;
            }
        }
        return null;
    }

    public void useItem()
    {
        inHand.stack--;
        if (inHand.stack == 0)
        {
            items[select] = null;
            inHand = items[select];
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }

    public int getNextFreeSpace()
    {
        for (int i = 0; i < items.Length - 5; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log(select);

            if (select < 9)
            {
                select++;
                inHand = items[select];
            }


            Debug.Log(inHand.itemName);
            Debug.Log(inHand.stack);
            Debug.Log(select);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (select > 0)
            {
                select--;
                inHand = items[select];
            }

            Debug.Log(inHand.itemName);
            Debug.Log(inHand.stack);
            Debug.Log(select);
        }
    }

    public void Craft()
    {
        craftingBook.checkForRecipe(items[40], items[41], items[42], items[43], items[44]);
        LocateZeroStacks();
        inventoryUI[0].UpdateUI();
        inventoryUI[1].UpdateUI();
        inHand = items[select];
    }

    public void UpdateItemInHand()
    {
        inHand = items[select];
    }

    public void Sort()
    {
        for (int i = 0; i < items.Length - 6; ++i)
        {
            if (items[i] != null)
            {
                for (int j = i + 1; j < items.Length - 5; ++j)
                {
                    if (items[j] != null && items[i].itemName.Equals(items[j].itemName))
                    {
                        items[i].stack += items[j].stack;
                        items[j] = null;
                    }
                }
            }
        }
    }

    public void LocateZeroStacks()
    {
        for (int i =40; i < items.Length; i++)
        {
            if (items[i]!=null && 0 >= items[i].stack)
            {
                items[i] = null;
            }
        }
    }
}
