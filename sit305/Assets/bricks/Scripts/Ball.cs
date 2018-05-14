using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    public Text scoreText;

    int score;

    Rigidbody2D ballRigidbody2D;
    CircleCollider2D ballCircleCollider2D;


    [Header("Horizontal speed")]
    public float speedX;

    [Header("Vertical speed")]
    public float speedY;

    enum tags
    {
        Bricks,
        background
    }

    void Start()
    {
        ballRigidbody2D = GetComponent<Rigidbody2D>();
        ballCircleCollider2D = GetComponent<CircleCollider2D>();
        //ballRigidbody2D.velocity = new Vector2(speedX, speedY);
        scoreText.text = "Current score :";
        Invoke("ballStart", 3);

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) )
        {
            ballStart();
        }
    }

    void ballStart()
        {
        if (isStop())
        {
            ballCircleCollider2D.enabled = true;
            transform.SetParent(null);
            ballRigidbody2D.velocity = new Vector2(speedX, speedY);
        }
        }
        bool isStop()
        {
        return ballRigidbody2D.velocity == Vector2.zero;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            lockSpeed();
            if (other.gameObject.CompareTag(tags.Bricks.ToString()))
            {
                other.gameObject.SetActive(false);
                score += 10;
                scoreText.text = "Current score :" + score;
            }
        }

        void lockSpeed()
        {
            Vector2 lockSpeed = new Vector2(resetSpeedX(), resetSpeedY());
            ballRigidbody2D.velocity = lockSpeed;
        }
        float resetSpeedX()
        {
            float currentSpeedX = ballRigidbody2D.velocity.x;
            if (currentSpeedX < 0)
            {
                return -speedX;

            }
            else
            {
                return speedX;
            }
        }
        float resetSpeedY()
        {
            float currentSpeedY = ballRigidbody2D.velocity.y;
            if (currentSpeedY < 0)
            {
                return -speedY;

            }
            else
            {
                return speedY;
            }
        }
    }


