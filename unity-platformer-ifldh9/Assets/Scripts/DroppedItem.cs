using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;
    [SerializeField] private float radius = 3f;
    private Player player;
    private Inventory inventory;

    private void Start()
    {
        if (inventory == null && player == null)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();
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

        Item newItem = Instantiate(item);
        item = newItem;
        Invoke("Despawn", 120);
    }

    private void Update()
    {
        if (inventory != null && player != null)
        {
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
