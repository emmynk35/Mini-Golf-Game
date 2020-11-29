using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Ctx;
    public static ScoreManager Score;
    
    void Start()
    {
        Ctx = this;
        Score = GameObject.Find("ScoreText").GetComponent<ScoreManager>();
    }

    void Update()
    {
        
    }
}
