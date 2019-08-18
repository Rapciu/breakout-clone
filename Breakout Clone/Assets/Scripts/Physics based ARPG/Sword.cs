using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Rigidbody2D rb;
    PolygonCollider2D swordColl;
    SpriteRenderer swordSprite;

    [SerializeField] GameObject player;

    PlayerTest playerComp;

    FixedJoint2D swordIdle;
    SliderJoint2D swordThrust;
    HingeJoint2D swordSwing;

    JointMotor2D swordMotor;

    public bool penetration = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("works");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swordColl = GetComponent<PolygonCollider2D>();
        swordSprite = GetComponent<SpriteRenderer>();

        playerComp = player.GetComponent<PlayerTest>();

        swordIdle = GetComponent<FixedJoint2D>();
        swordThrust = GetComponent<SliderJoint2D>();
        swordSwing = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 20)
        {
            //Debug.Log(rb.velocity.magnitude);

            //swordColl.enabled = false;
            Physics2D.IgnoreLayerCollision(8, 0, true);
            //swordSprite.color = new Color(255, 0, 0);

            penetration = true;
        }
        else if (rb.velocity.magnitude < 8 & penetration)
        {
            //swordColl.enabled = true;
            Physics2D.IgnoreLayerCollision(8, 0, false);
            //swordSprite.color = new Color(255, 255, 255);

            penetration = false;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
