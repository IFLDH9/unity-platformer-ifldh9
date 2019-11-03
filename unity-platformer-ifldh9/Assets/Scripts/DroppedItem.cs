using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;
    public float radius = 3f;
    GameObject player;
    Inventory inventory;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<Inventory>();
        Invoke("Despawn", 120);
        item.stack = 1;
    }

    void Update()
    {
        Vector3 diff = player.transform.position - transform.position;
        float distance = diff.sqrMagnitude;
        if(radius > distance)
        {
           if(inventory.Add(item))
            {
                Destroy(gameObject);
            }
        }
    }

    void Despawn()
    {
        Destroy(gameObject);
    }
}
