using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }else
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

    public void QuitAndSave()
    {
        SceneManager.LoadScene("MainMenu");
    }
 
}
