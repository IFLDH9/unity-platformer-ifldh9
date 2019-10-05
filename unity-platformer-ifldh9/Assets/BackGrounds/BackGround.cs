using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float scrollSpeed = 0.0001f;
    public Rigidbody2D body;

    private MeshRenderer mesh_Renderer;

    private Vector2 savedOffset;
    void Awake()
    {
        mesh_Renderer = GetComponent<MeshRenderer>();

        savedOffset = mesh_Renderer.sharedMaterial.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        if(body.velocity.x>0)
        {
            float x = Time.time *scrollSpeed;
            Vector2 offset = new Vector2(x, 0);
            mesh_Renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
       
        

     
    }
    void OnDisable()
    {
        mesh_Renderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
