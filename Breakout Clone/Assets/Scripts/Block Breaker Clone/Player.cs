using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float screenWidth = 32;
    [SerializeField] float screenHeight = 18;
    [SerializeField] float spriteWidth = 4;

    [SerializeField] GameObject ball;

    float spritePivot_x, num;

    Vector2 playerPos;
    Vector2 ballOffset;

    public Rigidbody2D rb;

    Ball ballComp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPos = new Vector2(transform.position.x, transform.position.y);
        spritePivot_x = spriteWidth / 2f;

        ballOffset = ball.transform.position - transform.position;

        ballComp = ball.GetComponent<Ball>();
    }

    void Move()
    {
        // Convert the mouse position from screen pixels to world units
        float mousePos_x = (Input.mousePosition.x / Screen.width) * screenWidth;

        //Vector2 playerPos = new Vector2(ballTransform.position.x + num, transform.position.y);
        playerPos.x = Mathf.Clamp(mousePos_x, spritePivot_x, screenWidth - spritePivot_x);
        //transform.position = playerPos;
        rb.MovePosition(playerPos);
    }

    public void SpawnBall()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 randomOffset = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

            GameObject spawnedBall = Instantiate(ball, (Vector2)transform.position + ballOffset + randomOffset, Quaternion.identity);
            //Ball spawnedBallComp = spawnedBall.GetComponent<Ball>();
            //FixedJoint2D spawnedBallAttachment = spawnedBall.GetComponent<FixedJoint2D>();

            //spawnedBallAttachment.connectedBody = rb;

            //spawnedBallComp.Launch();
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        num = Random.Range(-0.1f, 0.1f);
    }
    */
    // Update is called once per frame
    void Update()
    {
        Move();
        SpawnBall();
    }
}
