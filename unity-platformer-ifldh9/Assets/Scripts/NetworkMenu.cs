using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkMenu : MonoBehaviour
{
    public NetworkManager manager;
    [SerializeField] public bool showGUI = true;
    private TextMeshProUGUI text;
    public TextMeshProUGUI errorText;
    public Canvas canvas;
    public Button joinButton;
    public Button startServerButton;
    public Button hostServerButton;
    public GameObject loadingScreen;

   

    public GameObject loginScreen;
    void Awake()
    {
        if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
        {
            startServerButton.gameObject.SetActive(false);
            hostServerButton.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        text = loginScreen.GetComponentInChildren<TextMeshProUGUI>();
        manager = FindObjectOfType<NetworkManager>();
    }

   public void SetColor()
    {
       text.color=new Color32(29,30,80,255);
    }

   public void SetBackColor()
    {
        text.color = new Color32(255,255,255,255);
    }

    public void Update()
    {
        if (!showGUI)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.StartClient();
        }
    }

    public void JoinTheServer()
    {
        NetworkClient client = manager.StartClient();
        client.RegisterHandler(MsgType.Disconnect, ConnectionError);
        //client.RegisterHandler(MsgType.Connect, ConnectionSuccess);
        canvas.gameObject.SetActive(false);
        errorText.text = "Connecting...";
        loginScreen.SetActive(true);

       
    }

    public void ConnectionSuccess(NetworkMessage netMsg)
    {
        canvas.gameObject.SetActive(false);
        loginScreen.SetActive(true);
    }

    public void ConnectionError(NetworkMessage netMsg)
    {
        canvas.gameObject.SetActive(true);
        errorText.text = "Couldn't connect to the server.";
        manager.StopClient();
    }

    public void StartHost()
    {
        canvas.gameObject.SetActive(false);
        manager.StartHost();
    }

    public void StartServer()
    {
        canvas.gameObject.SetActive(false);
        manager.StartServer();
    }

}
