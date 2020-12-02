using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Ctx;
    public static ScoreManager Score;
    public UIManager UI;
    
    internal void Start()
    {
        Ctx = this;
        UI = new UIManager();
        UI.ShowStartMenu();
        Time.timeScale = 0f;
        Score = GameObject.Find("Score Text").GetComponent<ScoreManager>();
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        UI.HideStartMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UI.HidePauseMenu();
    }

    public void RestartGame()
    {
        // TODO: go back to level 1
        UnityEditor.EditorApplication.isPlaying = true;
        Time.timeScale = 1f;
        UI.HideRestartMenu();
    }
}
