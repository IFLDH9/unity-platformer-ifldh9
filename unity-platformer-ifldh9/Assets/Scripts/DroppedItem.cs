using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;
    public float radius = 3f;
    Player player;
    Inventory inventory;

    void Start()
    {
        if (inventory == null && player == null)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();

            Debug.Log("Ennyi playert talált: " + players.GetUpperBound(0));

            foreach (Player player in players)
            {
                if (player.isLocalPlayer)
                {
                    inventory = player.GetComponent<Inventory>();
                    this.player = player;
                    break;
                }
            }
        }


        Item newItem =Instantiate(item);
        item = newItem;
        Invoke("Despawn", 120);
    }

    void Update()
    {

       

        if (inventory != null && player != null)
        {
            Debug.Log("bent vagyunk :)");
        Vector3 diff = player.transform.position - transform.position;
        float distance = diff.sqrMagnitude;
        if (radius > distance)
        {
            if (inventory.Add(item))
            {
                inventory.inventoryUI[0].UpdateUI();
                inventory.inventoryUI[1].UpdateUI();
                Destroy(gameObject);
            }
        }
        }
    }

    void Despawn()
    {
        Destroy(gameObject);
    }
}
