using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Ctx;
    public static ScoreManager Score;
    public UIManager UI;
    public Ball ball;
    public static int level = 0;
    public static string[] Levels = new string[] {"Level 1", "Level 2", "Level 3", "Level 4", "Level 5"};
    private bool begin = true;
   
    internal void Start()
    {
        Ctx = this;
        UI = new UIManager();
        Time.timeScale = 1f;
        Score = GameObject.Find("Score Text").GetComponent<ScoreManager>();
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        if (level == 0 && begin) {
            begin = false;
            UI.ShowStartMenu();
        } 
    }

    public void NextLevel()
    {
        level += 1;
        if (level == 5) {
            UI.ShowRestartMenu();
        } else {
            SceneManager.LoadScene(Levels[level]);
        }
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void StartGame()
    {
        UI.HideStartMenu();
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UI.HidePauseMenu(); 
    }

    public void RestartGame()
    {
        UnityEditor.EditorApplication.isPlaying = true;
        Time.timeScale = 1f;
        UI.HideRestartMenu();
        level = -1;
        NextLevel();
    }
}
