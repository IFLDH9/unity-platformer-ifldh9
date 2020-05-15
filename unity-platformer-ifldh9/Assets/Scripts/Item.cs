using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public TileBase tile;
    public Sprite icon = null;
    public int stack = 0;
    [SerializeField] private float timer;
    public bool canBePutDown;
    public BlockType blocktype;

    public enum BlockType
    {
        TORCH, BACKGROUND, FOREGROUND
    }

    void PickUp()
    {
        Destroy(this);
    }
}
