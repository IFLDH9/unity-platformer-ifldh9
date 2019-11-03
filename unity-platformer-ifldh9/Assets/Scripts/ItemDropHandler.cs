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
        inventory = Inventory.instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        int id = eventData.pointerDrag.transform.GetComponent<ItemDragHandler>().id;

        Debug.Log(eventData.pointerDrag.transform.parent.GetChild(0).GetChild(id) + "drop to " + gameObject.name);
        InventorySlot slot1 = eventData.pointerDrag.transform.parent.GetChild(0).GetChild(id).GetComponent<InventorySlot>();
        InventorySlot slot2 = gameObject.GetComponent<InventorySlot>();

        if(slot1 != null && slot2 !=null)
        {
            Debug.Log(slot1.id + " " + slot2.id);
            swapItems(slot1, slot2);
            inventoryUI.UpdateUI();
        }
    }

    public void swapItems(InventorySlot slot1,InventorySlot slot2)
    {
        Item tempItem = inventory.items[slot1.id];
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
