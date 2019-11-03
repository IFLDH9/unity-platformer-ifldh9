using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Block/Tree")]
public class Tree : Block
{
    private void Awake()
    {
        breakable = true;
    }

    public override void Drop(Vector3 pos)
    {

    }
}
