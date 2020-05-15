using UnityEngine;
using UnityEngine.Networking;
public class GameManager : NetworkBehaviour
{
    public GameObject player;
    public ForegroundTileController map = null;
    public static GameManager instance = null;
    public Camera cam;
    public BackgroundTileController enviromentMap;
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

        map = GetComponent<ForegroundTileController>();
        enviromentMap = GetComponent<BackgroundTileController>();
        cam = Camera.main;
        lightController = GetComponent<LightController>();
    }

    private void InitGame()
    {
        if (isServer)
        {
            map.GenerateMap();
            enviromentMap.CreateEnviroment(map.foregroundTileMatrix, map.foregroundTilemap);
        }
    }

    public bool HasTileAround(Vector3Int posInt)
    {
        if (map.foregroundTilemap.HasTile(new Vector3Int(posInt.x + 1, posInt.y, posInt.z)) || map.foregroundTilemap.HasTile(new Vector3Int(posInt.x - 1, posInt.y, posInt.z)) ||
            map.foregroundTilemap.HasTile(new Vector3Int(posInt.x, posInt.y + 1, posInt.z)) || map.foregroundTilemap.HasTile(new Vector3Int(posInt.x, posInt.y - 1, posInt.z)))
        {
            return true;
        }
        return false;
    }


}







