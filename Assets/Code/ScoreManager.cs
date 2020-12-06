using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int CurrScore = 0;
    public int LevelScore { get; private set; }
    private AudioSource source;
    private static Text ScoreText;
    private static Text BestScoreText;
    private Text LevelText;
    
    void Start()
    {
        // DontDestroyOnLoad(transform.gameObject);
        ScoreText = GameObject.Find("Score Text").GetComponent<Text>();
        BestScoreText = GameObject.Find("Best Score Text").GetComponent<Text>();
        LevelText = GameObject.Find("Level Score Text").GetComponent<Text>();
        source = GetComponent<AudioSource>();
        LevelScore = 0;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = string.Format("Game Score: {0}", CurrScore);
        LevelText.text = string.Format("Level Score: {0}", LevelScore);
    }

    public void IncrementScore()
    {
        LevelScore += 1;
        CurrScore += 1;
        UpdateScore();
    }
}
