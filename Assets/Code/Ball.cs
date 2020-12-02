using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float TopBound;
    private float BottomBound;
    private float RightBound;
    private float LeftBound;
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 direction;
    private AudioSource source;
    
    void Start()
    {
        Vector2 bl = new Vector2(0f, 0f);
        Vector2 tr = new Vector2(Screen.width, Screen.height);
        var cam = Camera.main;
        TopBound = cam.ScreenToWorldPoint(tr).y;
        BottomBound = cam.ScreenToWorldPoint(bl).y;
        RightBound = cam.ScreenToWorldPoint(tr).x;
        LeftBound = cam.ScreenToWorldPoint(bl).x;
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        OutOfBoundsCheck();
        if (Input.GetKeyDown(KeyCode.P))
        {
            Game.Ctx.UI.ShowPauseMenu();
        }
        if (Input.GetMouseButtonDown(0)) {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp (0)) {
            endPos = Input.mousePosition;
            direction = startPos - endPos;
            rb.isKinematic = false;
            rb.AddForce(direction * 5f);
            source.PlayOneShot(source.clip, 1f);
        }
    }

    void OutOfBoundsCheck()
    {
        if (rb.position.x > RightBound || rb.position.x < LeftBound ||
            rb.position.y > TopBound || rb.position.y < BottomBound)
        {
            Debug.Log("out of bounds");
            //TODO bounce ball off walls
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var gameType = other.gameObject;
        if (gameType.GetComponent<Hole>())
        {
            Game.Score.IncrementScore();
            Debug.Log("ball in hole"); 
            //TODO increase score, move on to next level
        }
    }
}
