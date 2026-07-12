using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    public void PlayGame()
    {
        Debug.Log("Loading Game Scene...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Exiting Application...");
        Application.Quit();
    }
}