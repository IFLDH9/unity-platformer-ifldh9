using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] public static bool GameIsPaused = false;

    [SerializeField] private GameObject PauseMenuUI;

    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas login;
    [SerializeField] private Canvas registration;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !mainMenu.gameObject.activeSelf && !login.gameObject.activeSelf && !registration.gameObject.activeSelf)
        {
            if (AudioListener.volume == 0)
            {
                AudioListener.volume = 1;
            }
            else
            {
                AudioListener.volume = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenu.gameObject.activeSelf && !login.gameObject.activeSelf && !registration.gameObject.activeSelf)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void Quit()
    {
        CustomNetworkManager manager = FindObjectOfType<CustomNetworkManager>();
        manager.StopServer();
        manager.StopClient();
        manager.StopHost();
        Application.LoadLevel("Game");
    }

}
