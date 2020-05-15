using UnityEngine;

[CreateAssetMenu(fileName = "New Wood", menuName = "Block/Wood")]
public class Wood : Block
{
    [SerializeField] private GameObject droppedItem;
    private void Awake()
    {
        breakable = true;
    }

    public override void Drop(Vector3 pos)
    {
        DroppedItem item = droppedItem.GetComponent<DroppedItem>();
        item.item.stack = 1;
        Instantiate(droppedItem, pos, Quaternion.identity);
    }
}
