using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greg : MonoBehaviour
{

    PolygonCollider2D collider_1;
    FixedJoint2D fixedJoint;

    // Start is called before the first frame update
    void Start()
    {
        collider_1 = GetComponent<PolygonCollider2D>();
        fixedJoint = GetComponent<FixedJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Velocity:"+collision.rigidbody.velocity.magnitude);
        
        if (collision.rigidbody.velocity.magnitude > 6)
        {
            collider.enabled = false;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            fixedJoint.connectedBody = collision.rigidbody;
            fixedJoint.enabled = true;
            //collider.enabled = true;
        }
    }
    */
}
