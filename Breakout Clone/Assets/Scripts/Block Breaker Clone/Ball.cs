using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //[SerializeField] Transform playerTransComp;
    [SerializeField] float minVelocity = 10;
    [SerializeField] float maxVelocity = 25;
    [SerializeField] float launchPower = 25;
    [SerializeField] int launchAngleMaxOffset = 5;
    [SerializeField] float xMinVelocity = 5;
    [SerializeField] float yMinVelocity = 5;
    [SerializeField] int unstuckAngle = 5;

    [SerializeField] AudioClip[] collisionSounds;

    [SerializeField] AudioClip launchSound;

    Rigidbody2D rb;
    FixedJoint2D attachment;
    AudioSource collisionSound;

    GameObject player;

    Rigidbody2D playerRb;

    Vector2 velocityVector;

    float currentMaxVelocity;

    bool launched = false;

    //private void AttachToPaddle()
    //{
    //    Vector2 ballPos = (Vector2)player.transform.position + ballOffset;
    //    //transform.position = ballPos;
    //    rb.MovePosition(ballPos);
    //}

    public void LaunchBallUp()
    {
        rb.velocity = new Vector2(Random.Range(-launchAngleMaxOffset, launchAngleMaxOffset + 1), launchPower);
        //rb.AddForce(new Vector2(0, 200f));
    }

    //TODO: Move it to game manager script instead maybe
    public void Launch()
    {
        launched = true;
        attachment.enabled = false;
        LaunchBallUp();
        collisionSound.PlayOneShot(launchSound);
    }

    private void SetVelocity()
    {
        currentMaxVelocity = maxVelocity;

        if (Input.GetMouseButton(0))
        {
            currentMaxVelocity = maxVelocity * 2;
        }

        velocityVector = rb.velocity;

        if (velocityVector.magnitude > currentMaxVelocity)
        {
            rb.velocity = velocityVector.normalized * currentMaxVelocity;
        }
        else if (velocityVector.magnitude < minVelocity)
        {
            rb.velocity = velocityVector.normalized * minVelocity;
        }

        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void UnstuckBall()
    {
        velocityVector = rb.velocity;

        if (velocityVector.x < xMinVelocity && velocityVector.x > -xMinVelocity || velocityVector.y < yMinVelocity && velocityVector.y > -yMinVelocity)
        {
            velocityVector = Quaternion.Euler(0, 0, Random.Range(-unstuckAngle, unstuckAngle+1)) * velocityVector;

            rb.velocity = velocityVector;
        }
    }

    private void ballCollision()
    {
        //collisionSound.clip = collisionSounds[Random.Range(0, collisionSounds.Length)];
        //collisionSound.Play();

        AudioClip clip = collisionSounds[Random.Range(0, collisionSounds.Length)];
        collisionSound.pitch = Random.Range(0.5f, 1.5f);
        collisionSound.PlayOneShot(clip);

        rb.AddTorque(Random.Range(-10, 11));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (launched)
        {
            ballCollision();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attachment = GetComponent<FixedJoint2D>();
        collisionSound = GetComponent<AudioSource>();

        player = FindObjectOfType<Player>().gameObject;

        playerRb = player.GetComponent<Rigidbody2D>();

        attachment.connectedBody = playerRb;
        //attachment.anchor = new Vector2(0f, 2.5f);
        //attachment.connectedAnchor = new Vector2(0f, 0f);
    }

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        
        //ballOffset = transform.position - player.transform.position;
    }

    void Update()
    {
        if (!launched && Input.GetMouseButtonDown(0))
        {
            //AttachToPaddle();
            Launch();
        }
    }

    //Limits the ball velocity
    void FixedUpdate()
    {
        if (launched)
        {
            SetVelocity();
            UnstuckBall();
        }

        //Debug.Log($"x:{rb.velocity.x}\ny:{rb.velocity.y}");
    }
}
