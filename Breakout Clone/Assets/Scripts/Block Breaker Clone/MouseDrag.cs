using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 screenPoint;
    Vector3 offset;

    [SerializeField] float dragForceMultiplier = 1;

    void OnMouseDown()
    {
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    void OnMouseDrag()
    {
        Vector3 screenMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 dragHandlePos = Camera.main.ScreenToWorldPoint(screenMousePos) + offset;
        Vector2 velocityVector = dragHandlePos - gameObject.transform.position;
        rb.AddForce(velocityVector * dragForceMultiplier);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
}
