using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        if (GameManager.instance == null)
            Instantiate(gameManager);
    }

}
