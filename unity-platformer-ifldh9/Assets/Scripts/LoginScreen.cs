using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScreen : MonoBehaviour
{
    public bool login = false;
   public TMP_InputField nameField;
   public TMP_InputField passwordField;
   public Canvas mainMenu;
   public DatabaseHandler databaseHandler;
    public Canvas inventoryCanvas;
    public Canvas quickbarCanvas;


    public void Awake()
    {
        if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
        {
           // GetComponentInParent<Canvas>().gameObject.SetActive(false);
           // mainMenu.gameObject.SetActive(true);
        }
        passwordField.inputType = TMP_InputField.InputType.Password;
    }

    public void Start()
    {
        inventoryCanvas.gameObject.SetActive(false);
        quickbarCanvas.gameObject.SetActive(false);
    }   

    public void Update()
    {
        if (databaseHandler == null)
        {
            DatabaseHandler[] databaseHandlers = GameObject.FindObjectsOfType<DatabaseHandler>();

            Debug.Log("Ennyi playert talált: " + databaseHandlers.GetUpperBound(0));

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

    public void PlayGame()
    {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        mainMenu.gameObject.SetActive(false);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            yield return null;
        }
    }

    public void Login()
    {
        Debug.Log(nameField.text);
        Debug.Log(passwordField.text);
        databaseHandler.Login(nameField.text,passwordField.text);
        Debug.Log(login);
        if(login == true)
        {
            this.gameObject.SetActive(false);
        }

        Debug.Log("ennyi lett a login: " + login);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
