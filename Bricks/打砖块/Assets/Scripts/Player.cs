using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Horizontal speed")]
    public float speedX;
    Rigidbody2D playerRigidbody2D;

	void Start () {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        moveLeftOrRight();

    }

    float LeftOrRight()
    {
        return Input.GetAxis("Horizontal");
    }

    void moveLeftOrRight()
    {
        playerRigidbody2D.velocity = LeftOrRight() * new Vector2(speedX, 0);
    }
}
