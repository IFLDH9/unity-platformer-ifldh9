using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject inventoryUI;
    Inventory inventory;
    [SerializeField] private InventorySlot[] slots;
    TextMeshProUGUI[] texts;

    [SerializeField] private Transform craftingParent;
    InventorySlot[] craftingSlots;
    TextMeshProUGUI[] craftingTexts;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inventory != null)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryUI.SetActive(!inventoryUI.active);
                EventSystem es = EventSystem.current;
                EventSystem.current.SetSelectedGameObject(null);
                if (craftingParent != null)
                {
                    backFromCraftingPanel();
                }
                inventory.Sort();
                UpdateUI();
            }

            if (inventoryUI.active == true)
            {
                slots[inventory.select].button.Select();
            }
        }
        else
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
            if (inventory != null)
            {
                inventory.onItemChangedCallback += UpdateUI;
                slots = itemsParent.GetComponentsInChildren<InventorySlot>();
                texts = itemsParent.GetComponentsInChildren<TextMeshProUGUI>();
                if (craftingParent != null)
                {
                    craftingSlots = craftingParent.GetComponentsInChildren<InventorySlot>();
                    craftingTexts = craftingParent.GetComponentsInChildren<TextMeshProUGUI>();
                }
                UpdateUI();
            }
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
