using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Networking;

public class LightController : NetworkBehaviour
{

    [SerializeField] private Light2D globalLight2D;
    [SerializeField] private Light2D torchLight2D;
    [SerializeField] private List<Light2D> torches;

    [SyncVar]
    public bool afternoon = true;

    [SyncVar]
    public float dayNightTimer = 1.0f;

    void Start()
    {
        globalLight2D.intensity = 1;
    }

    void FixedUpdate()
    {
        TimePasses();
    }

    private void TimePasses()
    {
        if (afternoon)
        {
            dayNightTimer -= (Time.deltaTime / 300.0f) / 0.9f;

            if (dayNightTimer < 0.1f)
            {
                dayNightTimer = 0.1f;
                afternoon = false;
            }
            globalLight2D.intensity = dayNightTimer;
        }
        else
        {
            dayNightTimer += (Time.deltaTime / 300.0f) / 0.9f;
            if (dayNightTimer > 1.0f)
            {
                dayNightTimer = 1.0f;
                afternoon = true;
            }
            globalLight2D.intensity = dayNightTimer;
        }
    }

    public void PutDownTorch(Vector3Int pos)
    {
        Light2D torch = Instantiate(torchLight2D, pos, Quaternion.identity);
        torches.Add(torch);
    }

    public Light2D GetTorch(Vector3Int pos)
    {
        foreach (Light2D torch in torches)
        {
            if (torch.transform.position == pos)
            {
                return torch;
            }
        }
        return null;
    }

    public void RemoveTorch(Light2D torch)
    {
        torches.Remove(torch);
        Destroy(torch.gameObject);
    }

}
