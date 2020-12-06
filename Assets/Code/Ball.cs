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
    private bool mouseOnUIObject;
    private float circleSize;
    private float d_time;
    private Vector3 lastPos;
    
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
        circleSize = GetComponent<CircleCollider2D>().radius;
        lastPos = rb.position;
    }

    void FixedUpdate () {
        OutOfBoundsCheck();
    }

    void Update()
    {
        Debug.Log(Time.timeScale);
        mouseOnUIObject = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1f)
        {
            Game.Ctx.UI.ShowPauseMenu();
        }
		if (rb.velocity.magnitude > 0.2f)
		{
			sr.color = Color.red;
			return;
		} else {
            lastPos = rb.position;
        }
		sr.color = Color.white;
        if (Input.GetMouseButtonDown(0) && !mouseOnUIObject) {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && !mouseOnUIObject) {
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
        if (rb.position.x >= RightBound) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.left);
        } else if (rb.position.x <= LeftBound) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);
        } else if (rb.position.y >= TopBound) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.down);
        } else if (rb.position.y <= BottomBound) {
            rb.velocity = Vector3.Reflect(rb.velocity, Vector3.up);
        }
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Water") {
            Debug.Log("Splash");
            //gameType.PlaySplash();play sound from the water object
            Game.Score.IncrementScore();
            rb.position = lastPos;
            rb.velocity = Vector3.zero;
        }
    }
}
