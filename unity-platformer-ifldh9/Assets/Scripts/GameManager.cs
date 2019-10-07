using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.LWRP;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Map map = null;
    public static GameManager instance = null;
    public Camera cam;
    public EnviromentController enviromentMap;
    public Vector3Int focus;
    public Light2D globalLight2D;

    public float timer = 0;

    public bool afternoon = true;
    public float dayNightTimer = 1.0f;

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
       respawnPlayer();
      //  player.GetComponent<Transform>().position = new Vector3Int(0, 198, 0);
    }

    void InitGame()
    {

        map.GenerateMap();
        enviromentMap.createEnviroment(map.map);
    }

    
    void Update()
    {
        timePasses();
        Transform playerTrans = player.GetComponent<Transform>();
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            
            if (focus == posInt)
            {
                if (timer > 0.7f)
                {
                    Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
                    Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {0}]", pos.x, pos.y));
                    Debug.Log(string.Format("Co-ords of player is [X: {0} Y: {0}]", playerTrans.position.x, playerTrans.position.y));
                    Debug.Log(string.Format("distance {0}", Vector3.Distance(playerPos, pos)));
                    TileBase tile = map.tilemap.GetTile(posInt);
                    // Debug.Log(string.Format("tile {0}", tile.name));
                    Debug.Log(string.Format("tile {0}", map.map[posInt.x, posInt.y]));
                    if (map.tilemap.HasTile(posInt) && Vector3.Distance(playerPos, pos) < 3.0f)
                    {
                        //  Debug.Log(string.Format("poz {0}", posInt.x ));
                        map.map[posInt.x, posInt.y] = 0;
                        map.tilemap.SetTile(posInt, null);
                        map.UpdateMap(map.map, map.tilemap, posInt.x, posInt.y);
                    }
                    //waitingButtonUp = true;
                    timer = 0;
                }

            } else
            {
                focus.Set(posInt.x, posInt.y, posInt.z);
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
            Vector3 playerPos = new Vector3(playerTrans.position.x, playerTrans.position.y, 0);
            Vector3Int playerPosInt = new Vector3Int((int)playerTrans.position.x,(int)playerTrans.position.y,0);
            if (!map.tilemap.HasTile(posInt) && playerPosInt != posInt &&
                posInt != new Vector3Int(playerPosInt.x,playerPosInt.y+1,0) && hasTileAround(posInt))
            {
                map.map[posInt.x, posInt.y] = 1;
                map.tilemap.SetTile(posInt,map.dirtBlock);
            }
        }
    }

    public bool hasTileAround(Vector3Int posInt)
    {
        if(map.tilemap.HasTile(new Vector3Int(posInt.x+1,posInt.y,posInt.z)) || map.tilemap.HasTile(new Vector3Int(posInt.x-1, posInt.y, posInt.z)) ||
            map.tilemap.HasTile(new Vector3Int(posInt.x, posInt.y+1, posInt.z)) || map.tilemap.HasTile(new Vector3Int(posInt.x, posInt.y-1, posInt.z)))
        {
            return true;
        }
        return false;
    }

    public void respawnPlayer()
    {

        int randomX = 0;
        int randomY = 0;
        bool foundASpot = false;

        for(int i = 0; !foundASpot;++i)
        {

            randomX = Random.Range(0, 200);
            randomY = Random.Range(0, map.map.GetUpperBound(1) - 1);
            for (int y = randomY; y < map.map.GetUpperBound(1)-4; y++)
            {
                if(map.tilemap.HasTile(new Vector3Int(randomX,y,0)) && !map.tilemap.HasTile(new Vector3Int(randomX, y+1, 0))
                    )
            {
                    player.GetComponent<Transform>().position=new Vector3Int(randomX, y+1,0);
                    Debug.Log(string.Format("Co-ords [X: {0} Y: {0}]", randomX, y));
                    foundASpot = true;
                    break;
            }
            }

        } 
        }

    public void timePasses()
    {
        if(afternoon)
        {
            dayNightTimer -= (Time.deltaTime/720.0f)*0.8f;

            if(dayNightTimer<0.2f)
            {
                dayNightTimer = 0.2f;
                afternoon = false;
            }
            globalLight2D.intensity = dayNightTimer;
        }else
        {
            dayNightTimer += (Time.deltaTime / 720.0f)*0.8f;
            if (dayNightTimer > 1.0f)
            {
                dayNightTimer = 1.0f;
                afternoon = true;
            }
            globalLight2D.intensity = dayNightTimer;
        }
    }


    }


