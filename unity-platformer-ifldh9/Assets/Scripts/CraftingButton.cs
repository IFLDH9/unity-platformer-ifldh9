using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    public Button button;
    public Player player;

    public void Update()
    {
        if (player == null)
        {
            Player[] players = GameObject.FindObjectsOfType<Player>();

            foreach (Player player in players)
            {
                if (player.isLocalPlayer)
                {
                    this.player = player;
                    button.onClick.AddListener(player.GetComponent<Inventory>().Craft);
                    break;
                }
            }
        }
    }
}
