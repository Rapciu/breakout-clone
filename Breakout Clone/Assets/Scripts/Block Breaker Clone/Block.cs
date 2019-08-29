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

    [SerializeField] GameObject breakEffect;
    [SerializeField] AudioClip[] breakSounds;

    GameManager gameManagerComp;
    Level levelComp;
    //ParticleSystem breakEffectPSComp;
    //ParticleSystem.MainModule breakEffectPSMain;

    Rigidbody2D rb;
    SpriteRenderer sr;

    float angle;

    private void TriggerBreakEffect()
    {
        //TODO: Rewrite it so it doesn't change the prefab color directly DONE
        //breakEffectPSMain.startColor = sr.color;
        GameObject instantiatedBreakEffect = Instantiate(breakEffect, transform.position, transform.rotation);

        ParticleSystem breakEffectPSComp = instantiatedBreakEffect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule breakEffectPSMain = breakEffectPSComp.main;

        breakEffectPSMain.startColor = sr.color;

        Destroy(instantiatedBreakEffect, 5);
    }

    private void TriggerBreakSound()
    {
        AudioClip clip = breakSounds[Random.Range(0, breakSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.25f);
    }

    private void DestroyBlock()
    {
        gameManagerComp.AddPoints();

        if (gameManagerComp.showGUI)
        {
            gameManagerComp.DisplayScore();
        }

        TriggerBreakSound();

        Destroy(gameObject);

        TriggerBreakEffect();
    }

    private void Spin()
    {
        if (rb.bodyType != RigidbodyType2D.Kinematic)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        rb.MoveRotation(angle);
        angle += spinSpeed;
    }

    private void Start()
    {
        gameManagerComp = FindObjectOfType<GameManager>();
        levelComp = level.GetComponent<Level>();
        //breakEffectPSComp = breakEffect.GetComponent<ParticleSystem>();

        //breakEffectPSMain = breakEffectPSComp.main;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        angle = Random.Range(0, 90);
    }

    private void Update()
    {
        if (spin)
        {
            Spin();
        }
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
