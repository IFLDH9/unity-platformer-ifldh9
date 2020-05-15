using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkMenu : MonoBehaviour
{
    [SerializeField] private NetworkManager manager;
    [SerializeField] private bool showGUI = true;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button startServerButton;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Canvas registration;

    [SerializeField] private GameObject loginScreen;
    private void Awake()
    {
        if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
        {
            startServerButton.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        text = loginScreen.GetComponentInChildren<TextMeshProUGUI>();
        manager = FindObjectOfType<NetworkManager>();
    }

    public void SetColor()
    {
        text.color = new Color32(29, 30, 80, 255);
    }

    public void SetBackColor()
    {
        text.color = new Color32(255, 255, 255, 255);
    }

    public void Registration()
    {
        registration.gameObject.SetActive(true);
    }

    private void Update()
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

    public void StartServer()
    {
        manager.StartServer();
        canvas.gameObject.SetActive(false);
    }

}
