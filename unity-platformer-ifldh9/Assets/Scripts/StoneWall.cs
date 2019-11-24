using UnityEngine;

[CreateAssetMenu(fileName = "New Stone Wall", menuName = "Block/StoneWall")]
public class StoneWall : Block
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
