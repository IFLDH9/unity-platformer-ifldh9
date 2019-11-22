using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    Inventory inventory;
    InventorySlot[] slots;
    TextMeshProUGUI[] texts;

    public Transform craftingParent;
    InventorySlot[] craftingSlots;
    TextMeshProUGUI[] craftingTexts;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        texts = itemsParent.GetComponentsInChildren<TextMeshProUGUI>();
        Debug.Log(slots.Length + "length");
        if (craftingParent != null)
        {
            craftingSlots = craftingParent.GetComponentsInChildren<InventorySlot>();
            craftingTexts = craftingParent.GetComponentsInChildren<TextMeshProUGUI>();
            Debug.Log(craftingSlots.Length + "ez a hosssz");
        }
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(null);
            if (craftingParent != null)
            {
                backFromCraftingPanel();
            }
           inventory.Sort();
           UpdateUI();
        }
        
        if (inventoryUI.activeSelf)
        {
            slots[Inventory.select].button.Select();
        }
    }
    

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (inventory.items[i] == null)
            {
                slots[i].ClearSlot();
                texts[i].text = "";
            }
            else
            {
                slots[i].AddItem(inventory.items[i]);
                texts[i].text = inventory.items[i].stack.ToString();
            }
        }
        if (craftingParent != null)
        {
            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if (inventory.items[i + 40] == null)
                {
                    craftingSlots[i].ClearSlot();
                    craftingTexts[i].text = "";
                }
                else
                {
                    craftingSlots[i].AddItem(inventory.items[i + 40]);
                    craftingTexts[i].text = inventory.items[i + 40].stack.ToString();
                }
            }
        }
    }

    public void backFromCraftingPanel()
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].item != null)
            {
                inventory.items[i + 40] = null;
                inventory.Add(craftingSlots[i].item);
            }
        }
    }
}
