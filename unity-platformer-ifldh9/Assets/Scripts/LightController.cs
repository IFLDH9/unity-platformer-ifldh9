using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightController : MonoBehaviour
{

    public Light2D globalLight2D;
    public Light2D torchLight2D;
    public List<Light2D> torches;

    public bool afternoon = true;
    public float dayNightTimer = 1.0f;

    void Start()
    {
        globalLight2D.intensity = 1;
    }

    void Update()
    {
        timePasses();
    }

    public void timePasses()
    {
        if (afternoon)
        {
            dayNightTimer -= (Time.deltaTime / 50.0f) / 0.9f;

            if (dayNightTimer < 0.1f)
            {
                dayNightTimer = 0.1f;
                afternoon = false;
            }
            globalLight2D.intensity = dayNightTimer;
        }
        else
        {
            dayNightTimer += (Time.deltaTime / 50.0f)/ 0.9f;
            if (dayNightTimer > 1.0f)
            {
                dayNightTimer = 1.0f;
                afternoon = true;
            }
            globalLight2D.intensity = dayNightTimer;
        }
    }


    public void putDownTorch(Vector3Int pos)
    {
       Light2D torch = Instantiate(torchLight2D,pos,Quaternion.identity);
       torches.Add(torch);
    }

    public Light2D getTorch(Vector3Int pos)
    {
        foreach (Light2D torch in torches)
        {
            if(torch.transform.position == pos)
            {
                return torch;
            }
        }
        return null;
    }

    public void removeTorch(Light2D torch)
    {
        torches.Remove(torch);
        Destroy(torch.gameObject);
    }

}
