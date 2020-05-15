using UnityEngine;

[CreateAssetMenu(fileName = "New Stone Block", menuName = "Block/StoneBlock")]
public class StoneBlock : Block
{
    [SerializeField] private GameObject droppedItem;

    private void Awake()
    {
        breakable = true;
    }

    public override void Drop(Vector3 pos)
    {
        Instantiate(droppedItem, pos, Quaternion.identity);
    }
}