using TMPro;
using UnityEngine;

public class LoginScreen : MonoBehaviour
{
    [SerializeField] private bool login = false;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;
    public Canvas loginCanvas;
    [SerializeField] private DatabaseHandler databaseHandler;
    public Canvas inventoryCanvas;
    public Canvas quickbarCanvas;
    public TMP_Text errorText;
    [SerializeField] private Canvas mainMenu;

    private void Awake()
    {
        passwordField.inputType = TMP_InputField.InputType.Password;
    }

    public void Back()
    {
        CustomNetworkManager networkManager = FindObjectOfType<CustomNetworkManager>();
        networkManager.StopClient();
        mainMenu.gameObject.SetActive(true);
        loginCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (databaseHandler == null)
        {
            DatabaseHandler[] databaseHandlers = GameObject.FindObjectsOfType<DatabaseHandler>();

            foreach (DatabaseHandler database in databaseHandlers)
            {
                if (database.isLocalPlayer)
                {
                    databaseHandler = database;
                    database.loginScreen = mainMenu;
                    break;
                }
            }
        }
        if (login == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Login()
    {
        databaseHandler.CmdLogin(nameField.text, passwordField.text);
        if (login == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
