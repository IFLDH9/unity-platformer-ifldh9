using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public Item[] items = new Item[40];
    Item inHand = null;
    public static int select = 0;

    public Item Torch;

    public void Awake()
    {
        instance = this;
        Debug.Log("inventory :)");
        Torch.stack = 10;
        Debug.Log(select);
        Add(Torch);
        Debug.Log("added torch :)");
      //  Debug.Log(inHand.itemName);
        Debug.Log(select);
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;


    public bool Add(Item item)
    {
        Debug.Log(item.itemName + "has been added");
        Item listItem = GetItem(item);
        if(listItem != null)
        {
            listItem.stack += item.stack;
            return true;
        }
        else
        {
            int place = getNextFreeSpace();
            if (place>-1)
            {
                items[place] = item;

                if(onItemChangedCallback  != null)
                {
                   onItemChangedCallback.Invoke();
                }
                return true;
            }
            return false;
        }
    }

    public Item GetItem(Item item)
    {
        foreach (Item listItem in items)
        {
            if (listItem !=null && listItem.itemName.Equals(item.itemName))
            {
                return listItem;
            }
        }
        return null;
    }

    public void useItem()
    {
        if(inHand.canBePutDown==true)
        {
            inHand.stack--;
            if(inHand.stack == 0)
            {
                items[select] = null;
                if (onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }
            }
        }
    }

    public int getNextFreeSpace()
    {
        for (int i = 0; i < items.Length; i++)
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
            Debug.Log(select);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (select > 0)
            {
                select--;
                inHand = items[select];
            }

            Debug.Log(inHand.itemName);
            Debug.Log(select);
        }
    }

}
