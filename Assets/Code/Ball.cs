using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private AudioSource[] sources;
	private SpriteRenderer sr;
    private bool mouseOnButton;
    private float circleSize;
    private float d_time;
    private Vector3 lastPos;
    private LineRenderer lr;
    
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
        sources = gameObject.GetComponents<AudioSource>();
		sr = GetComponent<SpriteRenderer>();
        circleSize = GetComponent<CircleCollider2D>().radius;
        lastPos = rb.position;
        lr = GetComponent<LineRenderer>();
        mouseOnButton = false;
        Debug.Log(TopBound);
    }

    void FixedUpdate () {
        OutOfBoundsCheck();
    }

    void Update()
    {
        try {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()) {
                mouseOnButton = true;
            }
        } catch(Exception) {
            mouseOnButton = false;
        }
        
        if (Time.timeScale == 0f) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1f)
        {
            Game.Ctx.UI.ShowPauseMenu();
        }
		if (rb.velocity.magnitude > 0f)
		{
			sr.color = Color.red;
			return;
		} else {
            if (BoundsReset()) {
                rb.position = lastPos;
                rb.velocity = Vector3.zero;
                Game.Score.DecrementScore();
            }
            lastPos = rb.position;
            Debug.Log(lastPos);
        }
		sr.color = Color.white;
        if (Input.GetMouseButtonDown(0) && !mouseOnButton && Time.timeScale == 1f) {
            startPos = Input.mousePosition;
            lr.enabled = true;
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.SetPosition(0, rb.position);
        }
        if (Input.GetMouseButton(0) && !mouseOnButton && Time.timeScale == 1f)
        {
            Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(startPos);
            Vector3 worldv = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currPos = new Vector2(worldv.x, worldv.y);
            Vector2 diff = new Vector2(worldStartPos.x, worldStartPos.y) - currPos;
            Vector2 vec = rb.position + diff;
            lr.SetPosition(1, vec);
        }
        if (Input.GetMouseButtonUp(0) && !mouseOnButton && Time.timeScale == 1f) {
            endPos = Input.mousePosition;
            direction = startPos - endPos;
            rb.isKinematic = false;
            rb.AddForce(direction * 4f);
            sources[0].PlayOneShot(sources[0].clip, 1f);
            Game.Score.IncrementScore();
            lr.enabled = false;
        }
    }


    void OutOfBoundsCheck()
    {
        if (rb.position.x >= RightBound - 0.25f) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.left);
        } 
        if (rb.position.x <= LeftBound + 0.25f) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);
        }
        if (rb.position.y >= TopBound - 0.25f) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.down);
        }
        if (rb.position.y <= BottomBound + 0.25f) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.up);
        }
    }

    bool BoundsReset()
    {
        if (rb.position.x >= RightBound + 0.5f || rb.position.x <= LeftBound - 0.5f || rb.position.y >= TopBound + 0.5f || rb.position.y <= BottomBound - 0.5f) {
            return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var gameType = other.gameObject;
        if (gameType.GetComponent<Hole>())
        {
            RaycastHit2D hit = Physics2D.CircleCast(rb.position, (circleSize * 0.1f), rb.velocity, 0.1f);
            if (hit) {
                Game.Ctx.NextLevel();
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == "Sand") {
            rb.drag = 10f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sand") {
            rb.drag = 1.5f;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Water") {
            sources[2].PlayOneShot(sources[2].clip, 1f);
            Game.Score.IncrementScore();
            rb.position = lastPos;
            rb.velocity = Vector3.zero;
        }
        else if (other.gameObject.tag == "Obstacle") {
            sources[1].PlayOneShot(sources[1].clip, 1f);
        }
    }
}
