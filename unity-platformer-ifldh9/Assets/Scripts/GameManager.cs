using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class GameManager : NetworkBehaviour
{
    public GameObject player;
    public Map map = null;
    public static GameManager instance = null;
    public Camera cam;
    public EnviromentController enviromentMap;
    public Vector3Int focus;
    public LightController lightController;
    Inventory inventory;
    public float timer = 0;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        GameObject camera = GameObject.Find("Main Camera");
        camera.SetActive(false);
        InitGame();
    }

    public override void OnStartClient()
    {

        Debug.Log("csatlakozott");

        Debug.Log("csatlakozott2");
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        map = GetComponent<Map>();
        enviromentMap = GetComponent<EnviromentController>();
        cam = Camera.main;
        lightController = GetComponent<LightController>();
    }
    public void Start()
    {
       // InitGame();
       // RespawnPlayer();
      //  inventory = Inventory.instance;
    }

    void InitGame()
    {
        if (isServer)
        {
            Debug.Log("bentvagyokhelo");
            map.GenerateMap();
            enviromentMap.CreateEnviroment(map.map, map.tilemap);
        }   
    }

    public bool HasTileAround(Vector3Int posInt)
    {
        if (map.tilemap.HasTile(new Vector3Int(posInt.x + 1, posInt.y, posInt.z)) || map.tilemap.HasTile(new Vector3Int(posInt.x - 1, posInt.y, posInt.z)) ||
            map.tilemap.HasTile(new Vector3Int(posInt.x, posInt.y + 1, posInt.z)) || map.tilemap.HasTile(new Vector3Int(posInt.x, posInt.y - 1, posInt.z)))
        {
            return true;
        }
        return false;
    }

   
}







