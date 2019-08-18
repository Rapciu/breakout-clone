using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float screenWidth = 32;
    [SerializeField] float screenHeight = 18;
    [SerializeField] float spriteWidth = 4;
    [SerializeField] Transform ballTransform;

    float spritePivot_x, num;

    Vector2 playerPos;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPos = new Vector2(transform.position.x, transform.position.y);
        spritePivot_x = spriteWidth / 2f;
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
    }
}
