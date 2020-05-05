using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventoryUI inventoryUI;
    Inventory inventory;

    void Start()
    {
    }

    void Update()
    {
        if(inventory == null)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();

            Debug.Log("Ennyi playert talált: " + players.GetUpperBound(0));

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

    public void OnDrop(PointerEventData eventData)
    {
        int id = eventData.pointerDrag.transform.GetComponent<ItemDragHandler>().id;
       // Debug.Log(id);
       // Debug.Log(eventData.pointerDrag.transform.name);
        //Debug.Log(eventData.pointerDrag.transform.parent.GetChild(1).GetChild(id).name);

        // Debug.Log(eventData.pointerDrag.transform.parent.GetChild(0).GetChild(id) + "drop to " + gameObject.name);
        InventorySlot slot1 = null;
        try
        {
             slot1 = eventData.pointerDrag.transform.parent.GetChild(0).GetChild(id).GetComponent<InventorySlot>();
        }catch(Exception e)
        {
            slot1 = eventData.pointerDrag.transform.parent.GetChild(1).GetChild(id-40).GetComponent<InventorySlot>();
        }
        
        InventorySlot slot2 = gameObject.GetComponent<InventorySlot>();

        if(slot1 != null && slot2 !=null && slot2.id!=44)
        {
            Debug.Log(slot1.id + " " + slot2.id);
            swapItems(slot1, slot2);
            inventory.Sort();
            inventoryUI.UpdateUI();
            inventory.UpdateItemInHand();
        }
    }

    public void swapItems(InventorySlot slot1,InventorySlot slot2)
    {
        Item tempItem = inventory.items[slot1.id];
        Debug.Log(inventory.items.Length + "hossszzz");
        inventory.items[slot1.id] = inventory.items[slot2.id];
        inventory.items[slot2.id] = tempItem;

         tempItem = slot1.item;
        if(slot2.item !=null)
        {
            slot1.AddItem(slot2.item);
        }
        else
        {
            slot1.ClearSlot();
        }

        if (tempItem != null)
        {
            slot2.AddItem(tempItem);
        }
        else
        {
            slot2.ClearSlot();
        }
    }
}
