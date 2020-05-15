using UnityEngine;

[CreateAssetMenu(fileName = "New Torch", menuName = "Block/Torch")]
public class Torch : Block
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
