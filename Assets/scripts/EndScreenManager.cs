using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    // השמות של הסצינות שלך כפי שהן מופיעות בפרויקט
    public string mainMenuSceneName = "MainMenu";
    public string gameSceneName = "GameScene";

    // יקרא על ידי כפתור "חזרה להתחלה"
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // יקרא על ידי כפתור "משחק חדש" (הפעלה מחדש של המשחק)
    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}