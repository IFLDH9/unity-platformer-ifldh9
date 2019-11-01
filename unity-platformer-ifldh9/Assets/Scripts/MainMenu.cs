using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public GameObject mainMenu;
   public GameObject loadingScreen;

   public void PlayGame()
    {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
