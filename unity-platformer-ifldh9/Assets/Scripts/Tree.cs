using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Block/Tree")]
public class Tree : Block
{
    public GameObject[] droppedItem;
    private void Awake()
    {
        breakable = true;
    }

    public override void Drop(Vector3 pos)
    {
        DroppedItem item = droppedItem[0].GetComponent<DroppedItem>();
        item.item.stack = Random.Range(1,6);
        Instantiate(droppedItem[Random.Range(0,droppedItem.Length)], pos, Quaternion.identity);
    }
}
