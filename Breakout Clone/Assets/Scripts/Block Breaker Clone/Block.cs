using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //[SerializeField] GameObject gameManager;
    [SerializeField] GameObject level;

    [SerializeField] bool breakable = true;

    [SerializeField] AudioClip[] breakSounds;

    GameManager gameManagerComp;
    Level levelComp;

    private void DestroyBlock()
    {
        gameManagerComp.AddPoints();

        AudioClip clip = breakSounds[Random.Range(0, breakSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.25f);

        Destroy(gameObject);
    }

    private void Start()
    {
        gameManagerComp = FindObjectOfType<GameManager>();
        levelComp = level.GetComponent<Level>();
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
