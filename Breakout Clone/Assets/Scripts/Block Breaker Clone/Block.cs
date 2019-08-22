using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //[SerializeField] GameObject gameManager;
    [SerializeField] GameObject level;

    [SerializeField] bool breakable = true;
    [SerializeField] bool spin = false;
    [Range(1f, 360f)] [SerializeField] float spinSpeed = 1f;

    [SerializeField] AudioClip[] breakSounds;

    GameManager gameManagerComp;
    Level levelComp;

    Rigidbody2D rb;

    float angle;

    private void DestroyBlock()
    {
        gameManagerComp.AddPoints();

        if (gameManagerComp.showGUI)
        {
            gameManagerComp.DisplayScore();
        }

        AudioClip clip = breakSounds[Random.Range(0, breakSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.25f);

        Destroy(gameObject);
    }

    private void Spin()
    {
        if (spin)
        {
            if (rb.bodyType != RigidbodyType2D.Kinematic)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            rb.MoveRotation(angle);
            angle += spinSpeed;
        }
    }

    private void Start()
    {
        gameManagerComp = FindObjectOfType<GameManager>();
        levelComp = level.GetComponent<Level>();

        rb = GetComponent<Rigidbody2D>();

        angle = Random.Range(0, 90);
    }

    private void Update()
    {
        Spin();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (breakable)
        {
            DestroyBlock();

            //levelComp.CheckBlocks();
        }
    }
}
