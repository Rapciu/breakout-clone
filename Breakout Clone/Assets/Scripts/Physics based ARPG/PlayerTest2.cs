using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTest2 : MonoBehaviour
{
    [SerializeField] float forcePower = 500f;
    [SerializeField] float torquePowerFactor;
    //[SerializeField] float maxForceSpeed = 1f;
    //[SerializeField] float maxTorqueSpeed = 1f;
    [SerializeField] int mouseFollowWiggleRoom;

    //[SerializeField] GameObject sword;
    //[SerializeField] SliderJoint2D swordThrust;
    [SerializeField] GameObject swordIdle;
    [SerializeField] GameObject swordThrust;
    [SerializeField] GameObject swordSwing;

    [SerializeField] float maxAttackSpeed = 100;

    [SerializeField] TextMeshProUGUI thrustPowerText;

    Camera cam;
    Rigidbody2D rb;

    FixedJoint2D swordIdleComp;
    SliderJoint2D swordThrustComp;
    HingeJoint2D swordSwingComp;

    Vector2 mouseWorldPos;

    Vector2 forceDirection = new Vector2(0, 0);
    Vector2 mouseDirection = new Vector2(0, 0);

    JointMotor2D swordMotor;
    JointAngleLimits2D swordSwingLimits;

    float attackSpeed = 0;

    float mousePlayerAngle;

    private void Move()
    {
        //Using the unity input system
        forceDirection.x = Input.GetAxis("Horizontal");
        forceDirection.y = Input.GetAxis("Vertical");
        forceDirection.Normalize();
        rb.AddForce(forceDirection * forcePower);
    }

    private void FaceMouse()
    {
        mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        mouseDirection.x = mouseWorldPos.x - transform.position.x;
        mouseDirection.y = mouseWorldPos.y - transform.position.y;

        //float mousePlayerAngleOffset = Vector2.Distance((Vector2)transform.up.normalized, mouseDirection.normalized);

        //float mousePlayerAngle = Vector2.Angle((Vector2)transform.up, mouseDirection);
        //Signed angle reyurns the angle between -180 and 180 instead of an absolute value like Vector2.Angle
        mousePlayerAngle = Vector2.SignedAngle((Vector2)transform.up, mouseDirection);

        //Debug.Log(mousePlayerAngle);

        //Rotate
        if (mousePlayerAngle > mouseFollowWiggleRoom)
        {
            rb.AddTorque(torquePowerFactor * Mathf.Abs(mousePlayerAngle));
        }
        if (mousePlayerAngle < -mouseFollowWiggleRoom)
        {
            rb.AddTorque(-torquePowerFactor * Mathf.Abs(mousePlayerAngle));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = new Vector2(0, 0);
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        swordIdleComp = swordIdle.GetComponent<FixedJoint2D>();
        swordThrustComp = swordThrust.GetComponent<SliderJoint2D>();
        swordSwingComp = swordSwing.GetComponent<HingeJoint2D>();

        //swordIdle.enabled = false;
        //swordThrust.enabled = false;
        //swordSwing.enabled = false;

        thrustPowerText.text = attackSpeed.ToString();
    }

    // FixedUpdate is used to keep the commands in sync with the physics engine
    void FixedUpdate()
    {
        /*
        //TODO: Use a for loop to loop through a dictionary with Keycodes as keys and directions as values. And check if the key is pressed.
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector2.up * forcePower);
            //rb.AddRelativeForce(Vector2.up * forcePower);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector2.down * forcePower);
            //rb.AddRelativeForce(Vector2.down * forcePower);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * forcePower);
            //rb.AddRelativeForce(Vector2.left * forcePower);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * forcePower);
            //rb.AddRelativeForce(Vector2.right * forcePower);
        }
        */

        //Move
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || 
            Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Move();
        }

        FaceMouse();

        /*
         * Don't use that, it limits the general velocity which will limit how the player is moved by external forces
        if (rb.velocity.magnitude > maxForceSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxForceSpeed;
        }
        if (rb.angularVelocity > maxTorqueSpeed)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxTorqueSpeed;
        }
        */
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //swordThrust.enabled = true;
            //swordSwing.enabled = false;
            //swordIdle.enabled = false;

            swordMotor = swordThrustComp.motor;
            swordMotor.motorSpeed = (10 / maxAttackSpeed) * 8;
            swordMotor.maxMotorTorque = 1000;
            swordThrustComp.motor = swordMotor;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (attackSpeed < maxAttackSpeed)
            {
                attackSpeed += 1f;
            }
            
            //Debug.Log(attackSpeed);
            thrustPowerText.text = attackSpeed.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            swordMotor = swordThrustComp.motor;
            swordMotor.motorSpeed = -attackSpeed;
            swordMotor.maxMotorTorque = 3000;
            swordThrustComp.motor = swordMotor;
            attackSpeed = 0f;
            thrustPowerText.text = attackSpeed.ToString();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            /*
            swordSwingLimits = swordSwing.limits;
            swordSwingLimits.min = 330; //315
            swordSwingLimits.max = 450;
            swordSwing.limits = swordSwingLimits;
            */
            //swordSwing.enabled = true;
            //swordThrust.enabled = false;
            //swordIdle.enabled = false;
            /*
            Vector2 swordPos = transform.position;
            swordPos.y += 0.75f;
            sword.transform.position = swordPos;
            sword.transform.rotation = transform.rotation;
            */
            swordMotor = swordSwingComp.motor;
            swordMotor.motorSpeed = (1000 / maxAttackSpeed) * 10;
            swordMotor.maxMotorTorque = 1000;
            swordSwingComp.motor = swordMotor;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (attackSpeed < maxAttackSpeed)
            {
                attackSpeed += 1f;
            }

            //Debug.Log(attackSpeed);
            thrustPowerText.text = attackSpeed.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            swordMotor = swordSwingComp.motor;
            swordMotor.motorSpeed = -(attackSpeed * 20);
            swordMotor.maxMotorTorque = 3000;
            swordSwingComp.motor = swordMotor;
            attackSpeed = 0f;
            thrustPowerText.text = attackSpeed.ToString();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            /*
            swordSwingLimits = swordSwing.limits;
            swordSwingLimits.min = 270;
            swordSwingLimits.max = 390; //405
            swordSwing.limits = swordSwingLimits;
            */
            //swordSwing.enabled = true;
            //swordThrust.enabled = false;
            //swordIdle.enabled = false;

            swordMotor = swordSwingComp.motor;
            swordMotor.motorSpeed = -((1000 / maxAttackSpeed) * 10);
            swordMotor.maxMotorTorque = 1000;
            swordSwingComp.motor = swordMotor;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (attackSpeed < maxAttackSpeed)
            {
                attackSpeed += 1f;
            }

            //Debug.Log(attackSpeed);
            thrustPowerText.text = attackSpeed.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            swordMotor = swordSwingComp.motor;
            swordMotor.motorSpeed = attackSpeed * 20;
            swordMotor.maxMotorTorque = 3000;
            swordSwingComp.motor = swordMotor;
            attackSpeed = 0f;
            thrustPowerText.text = attackSpeed.ToString();
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            swordMotor = swordThrustComp.motor;
            swordMotor.motorSpeed = 0;
            swordMotor.maxMotorTorque = 100;
            swordThrustComp.motor = swordMotor;
            swordMotor = swordSwingComp.motor;
            swordMotor.motorSpeed = 0;
            swordMotor.maxMotorTorque = 100;
            swordSwingComp.motor = swordMotor;
            //swordIdle.enabled = true;
            //swordThrust.enabled = false;
            //swordSwing.enabled = false;
        }
    }
}
