using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName ="Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public TileBase tile;
    public Sprite icon = null;
    public int stack = 0;
    public float timer;
    public bool canBePutDown;

    public void Update()
    {

    }

    void PickUp()
    {
        Destroy(this);
    }
}
