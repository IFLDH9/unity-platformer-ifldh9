using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    Inventory inventory;
    InventorySlot[] slots; 
    void Start()
    {
       inventory = Inventory.instance;
       inventory.onItemChangedCallback += UpdateUI;
       slots = itemsParent.GetComponentsInChildren<InventorySlot>();
       UpdateUI();
    }

    void Update()
    {
        
        if(Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            UpdateUI();
            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(null);
        }
        slots[Inventory.select].button.Select();
    }

   public void UpdateUI()
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if(inventory.items[i] == null)
            {
                slots[i].ClearSlot();
            }else
            {
                slots[i].AddItem(inventory.items[i]);
            }
        }
    }
}
