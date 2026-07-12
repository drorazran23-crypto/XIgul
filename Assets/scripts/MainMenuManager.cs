using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // שם הסצינה של המשחק (ודאי שהשם תואם ב-Build Settings)
    public string gameSceneName = "GameScene";

    // פונקציה לכפתור להתחלת המשחק
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // פונקציה לכפתור יציאה מהמשחק
    public void QuitGame()
    {
        Debug.Log("Game is Exiting...");
        Application.Quit();
    }
}