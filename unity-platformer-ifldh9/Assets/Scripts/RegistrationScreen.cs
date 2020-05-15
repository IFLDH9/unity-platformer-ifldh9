using TMPro;
using UnityEngine;

public class RegistrationScreen : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField nicknameField;
    [SerializeField] private DatabaseHandler databaseHandler;
    public TMP_Text result;
    [SerializeField] private Canvas self;

    public void Register()
    {
        if (databaseHandler.isLocalPlayer)
        {
            databaseHandler.CmdSignUp(nameField.text, passwordField.text, nicknameField.text);
        }

    }

    public void Back()
    {
        CustomNetworkManager networkManager = FindObjectOfType<CustomNetworkManager>();
        networkManager.StopClient();
        self.gameObject.SetActive(false);
    }

    void Start()
    {
        CustomNetworkManager networkManager = FindObjectOfType<CustomNetworkManager>();
        networkManager.StartClient();
        passwordField.inputType = TMP_InputField.InputType.Password;
    }

    void Update()
    {
        if (databaseHandler == null)
        {
            DatabaseHandler[] databaseHandlers = GameObject.FindObjectsOfType<DatabaseHandler>();
            foreach (DatabaseHandler database in databaseHandlers)
            {
                if (database.isLocalPlayer)
                {
                    databaseHandler = database;
                    break;
                }
            }
        }
    }
}
