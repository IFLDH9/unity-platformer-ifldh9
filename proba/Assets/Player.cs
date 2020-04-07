using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ClientRpc]
    void Rpchaha()
    {
        Debug.Log("haha");
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Rpchaha();

        }
    }
}
