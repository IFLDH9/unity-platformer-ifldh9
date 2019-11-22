using UnityEngine;

[CreateAssetMenu(fileName = "New Wood Wall", menuName = "Block/WoodWall")]
public class WoodWall : Block
{
    public GameObject droppedItem;
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
