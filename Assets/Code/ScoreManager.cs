﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int CurrScore { get; private set; }
    private AudioSource source;
    private static Text ScoreText;
    
    void Start()
    {
        ScoreText = GetComponent<Text>();
        source = GetComponent<AudioSource>();
        CurrScore = 0;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = string.Format("Score: {0}", CurrScore);
    }

    public void IncrementScore()
    {
        CurrScore += 1;
        UpdateScore();
    }
}
