using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Map map = null;
    public static GameManager instance = null;
    public Camera cam;
    public EnviromentController enviromentMap;
    public Vector3Int focus;

    public float timer = 0;
    public bool waitingButtonUp = false;

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

        DontDestroyOnLoad(gameObject);
        // player = GetComponent<Transform>();
        map = GameObject.Find("Map").GetComponent<Map>();
        enviromentMap = GameObject.Find("EnviromentMap").GetComponent<EnviromentController>();
        cam = Camera.main;
       InitGame();
    }

    void InitGame()
    {

        map.GenerateMap();
        enviromentMap.createEnviroment(map.map);
    }

    
    void Update()
    {

        if(Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            if(focus == posInt)
            { 
                if(timer >0.7f)
                {
                     Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
                        Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {0}]", pos.x, pos.y));
                     Debug.Log(string.Format("Co-ords of player is [X: {0} Y: {0}]", player.transform.position.x,player.transform.position.y));
                        Debug.Log(string.Format("distance {0}", Vector3.Distance(playerPos, pos)));
                        TileBase tile = map.tilemap.GetTile(posInt);
                         if (map.tilemap.HasTile(posInt) && Vector3.Distance(playerPos,pos)<3.0f)
            {
                map.map[posInt.x,posInt.y] = 0;
            }
                //waitingButtonUp = true;
                timer = 0;
                }
           
            } else
            {
                focus.Set(posInt.x,posInt.y,posInt.z);
                timer = 0;
            }
        }
        else
        {
            timer = 0;
          //  waitingButtonUp = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            Vector3Int playerPosInt = new Vector3Int((int)player.transform.position.x,(int)player.transform.position.y,0);
            if (!map.tilemap.HasTile(posInt) && playerPosInt != posInt && posInt != new Vector3Int(playerPosInt.x,playerPosInt.y+1,0))
            {
                map.map[posInt.x, posInt.y] = 1;
                map.tilemap.SetTile(posInt,map.dirtBlock);
            }
        }


    }


}
