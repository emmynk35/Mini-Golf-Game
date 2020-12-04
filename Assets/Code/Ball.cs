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
	private SpriteRenderer sr;
    
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
		sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
		if (rb.velocity.x > 0.1f || rb.velocity.y > 0.1f)
		{
			sr.color = Color.red;
			return;
		}
		sr.color = Color.white;
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
			Game.Score.IncrementScore();
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
            Debug.Log("ball in hole"); 
            //TODO move on to next level
        }
    }
}
