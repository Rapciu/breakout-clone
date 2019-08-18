using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetration : MonoBehaviour
{
    GameObject sword;
    Sword swordComp;

    FixedJoint2D penetrationWeld;

    List<FixedJoint2D> penetrationWelds = new List<FixedJoint2D>();

    Rigidbody2D swordRb;

    private void Start()
    {
        sword = transform.parent.gameObject;
        swordComp = sword.GetComponent<Sword>();
        swordRb = sword.GetComponent<Rigidbody2D>();
    }
    
private void OnTriggerEnter2D(Collider2D collision)
    {
        if (swordComp.penetration & collision.gameObject.layer == 0)
        {
            Debug.Log("penetrated");

            penetrationWeld = sword.AddComponent<FixedJoint2D>();
            penetrationWelds.Add(penetrationWeld);

            penetrationWeld.connectedBody = collision.attachedRigidbody;
            penetrationWeld.dampingRatio = 1;
            penetrationWeld.frequency = 0;

            //penetrationWeld.dampingRatio = 0.8f;
            //penetrationWeld.frequency = 15;
            penetrationWeld.breakForce = 10000;
            penetrationWeld.breakTorque = 10000;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //penetrationWeld.breakForce = 1000;
        //penetrationWeld.breakTorque = 1000;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            for(int weld = 0; weld < penetrationWelds.Count; weld++)
            {
                if (penetrationWelds[weld] != null)
                {
                    penetrationWelds[weld].dampingRatio = 0.6f;
                    penetrationWelds[weld].frequency = 20;
                    penetrationWelds[weld].breakForce = 10;
                    penetrationWelds[weld].breakTorque = 10;
                }
                else if (penetrationWelds[weld] == null)
                {
                    penetrationWelds.RemoveAt(weld);
                }
            }

            /*
            penetrationWeld.dampingRatio = 0.6f;
            penetrationWeld.frequency = 20;
            penetrationWeld.breakForce = 10;
            penetrationWeld.breakTorque = 10;
            */
        }

        else if (swordRb.velocity.magnitude < 5)
        {
            for (int weld = 0; weld < penetrationWelds.Count; weld++)
            {
                if (penetrationWelds[weld] != null)
                {
                    penetrationWelds[weld].breakForce = 5000;
                    penetrationWelds[weld].breakTorque = 5000;
                }
                else if (penetrationWelds[weld] == null)
                {
                    penetrationWelds.RemoveAt(weld);
                }
            }

            //penetrationWeld.dampingRatio = 1;
            //penetrationWeld.frequency = 0;

            //penetrationWeld.breakForce = 5000;
            //penetrationWeld.breakTorque = 5000;
        }
    }
}
