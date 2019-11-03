
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int id;
    public Image icon;
    public Button button;
    public Item item;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        icon.color = new Color32(255, 255, 255, 255);
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }


}
