using UnityEngine;

[CreateAssetMenu(fileName = "New DirtBlock", menuName = "Block/DirtBlock")]
public class DirtBlock : Block
{
    public GameObject droppedItem;

    private void Awake()
    {
        breakable = true;       
    }

    public override void Drop(Vector3 pos)
    {
        Instantiate(droppedItem, pos, Quaternion.identity);
    }
}

