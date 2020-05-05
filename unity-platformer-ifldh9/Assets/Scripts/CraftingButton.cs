using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
   public Button button;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if ( player == null)
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
