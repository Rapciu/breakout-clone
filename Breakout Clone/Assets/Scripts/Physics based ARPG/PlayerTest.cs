using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] float movementSpeed = 500f;
    [SerializeField] float mouseFollowRotationSpeedFactor = 20;
    //[SerializeField] float maxForceSpeed = 1f;
    //[SerializeField] float maxTorqueSpeed = 1f;
    [SerializeField] int mouseFollowWiggleRoom;

    [SerializeField] GameObject sword;
    //[SerializeField] SliderJoint2D swordThrust;

    //TODO: Add a stamina bar
    [SerializeField] float maxAttackSpeed = 100;
    [SerializeField] float attackPower = 1000;

    [SerializeField] TextMeshProUGUI thrustPowerText;

    Camera cam;

    Rigidbody2D rb;
    Rigidbody2D swordRb;

    FixedJoint2D swordIdle;
    SliderJoint2D swordThrust;
    HingeJoint2D swordSwing;

    Sword swordComp;

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
        rb.AddForce(forceDirection * movementSpeed);
    }

    private void FaceMouse()
    {
        mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        mouseDirection.x = mouseWorldPos.x - transform.position.x;
        mouseDirection.y = mouseWorldPos.y - transform.position.y;

        //float mousePlayerAngleOffset = Vector2.Distance((Vector2)transform.up.normalized, mouseDirection.normalized);

        //float mousePlayerAngle = Vector2.Angle((Vector2)transform.up, mouseDirection);
        //Signed angle returns the angle between -180 and 180 instead of an absolute value like Vector2.Angle
        mousePlayerAngle = Vector2.SignedAngle((Vector2)transform.up, mouseDirection);

        //Debug.Log(mousePlayerAngle);

        //Rotate
        if (mousePlayerAngle > mouseFollowWiggleRoom)
        {
            rb.AddTorque(mouseFollowRotationSpeedFactor * Mathf.Abs(mousePlayerAngle));
        }
        if (mousePlayerAngle < -mouseFollowWiggleRoom)
        {
            rb.AddTorque(-mouseFollowRotationSpeedFactor * Mathf.Abs(mousePlayerAngle));
        }
    }

    private void AddAttackSpeed()
    {
        if (attackSpeed < maxAttackSpeed)
        {
            attackSpeed += 1f;
        }

        //Debug.Log(attackSpeed);
        thrustPowerText.text = attackSpeed.ToString();
    }

    private void ChargeThrust()
    {
        swordThrust.enabled = true;
        swordSwing.enabled = false;
        swordIdle.enabled = false;

        swordMotor = swordThrust.motor;
        swordMotor.motorSpeed = (10 / maxAttackSpeed) * 8;
        swordMotor.maxMotorTorque = attackPower / 2;
        swordThrust.motor = swordMotor;
    }
    private void Thrust()
    {
        swordMotor = swordThrust.motor;
        swordMotor.motorSpeed = -attackSpeed;
        swordMotor.maxMotorTorque = attackPower;
        swordThrust.motor = swordMotor;
        attackSpeed = 0f;
        thrustPowerText.text = attackSpeed.ToString();
    }

    private void ChargeSwingLeft()
    {
        /*
        swordSwingLimits = swordSwing.limits;
        swordSwingLimits.min = 330; //315
        swordSwingLimits.max = 450;
        swordSwing.limits = swordSwingLimits;
        */

        swordSwing.enabled = true;
        swordThrust.enabled = false;
        swordIdle.enabled = false;

        swordMotor = swordSwing.motor;
        swordMotor.motorSpeed = (1000 / maxAttackSpeed) * 15;
        swordMotor.maxMotorTorque = attackPower / 2;
        swordSwing.motor = swordMotor;
    }
    private void SwingLeft()
    {
        swordMotor = swordSwing.motor;
        swordMotor.motorSpeed = -(attackSpeed * 20);
        swordMotor.maxMotorTorque = attackPower;
        swordSwing.motor = swordMotor;
        attackSpeed = 0f;
        thrustPowerText.text = attackSpeed.ToString();
    }

    private void ChargeSwingRight()
    {
        /*
        swordSwingLimits = swordSwing.limits;
        swordSwingLimits.min = 270;
        swordSwingLimits.max = 390; //405
        swordSwing.limits = swordSwingLimits;
        */

        swordSwing.enabled = true;
        swordThrust.enabled = false;
        swordIdle.enabled = false;

        swordMotor = swordSwing.motor;
        swordMotor.motorSpeed = -((1000 / maxAttackSpeed) * 15);
        swordMotor.maxMotorTorque = attackPower / 2;
        swordSwing.motor = swordMotor;
    }
    private void SwingRight()
    {
        swordMotor = swordSwing.motor;
        swordMotor.motorSpeed = attackSpeed * 20;
        swordMotor.maxMotorTorque = attackPower;
        swordSwing.motor = swordMotor;
        attackSpeed = 0f;
        thrustPowerText.text = attackSpeed.ToString();
    }

    private void Idle()
    {
        swordMotor = swordThrust.motor;
        swordMotor.motorSpeed = 0;
        swordMotor.maxMotorTorque = 100;
        swordThrust.motor = swordMotor;
        swordMotor = swordSwing.motor;
        swordMotor.motorSpeed = 0;
        swordMotor.maxMotorTorque = 100;
        swordSwing.motor = swordMotor;
        swordIdle.enabled = true;
        swordThrust.enabled = false;
        swordSwing.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = new Vector2(0, 0);
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        swordRb = sword.GetComponent<Rigidbody2D>();

        swordIdle = sword.GetComponent<FixedJoint2D>();
        swordThrust = sword.GetComponent<SliderJoint2D>();
        swordSwing = sword.GetComponent<HingeJoint2D>();

        swordComp = sword.GetComponent<Sword>();

        //swordIdle.enabled = false;
        swordThrust.enabled = false;
        swordSwing.enabled = false;

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
            ChargeThrust();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            AddAttackSpeed();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Thrust();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChargeSwingLeft();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            AddAttackSpeed();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SwingLeft();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ChargeSwingRight();
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            AddAttackSpeed();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            SwingRight();
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Idle();
        }
        // || swordComp.penetration
        else if (swordRb.velocity.magnitude < 1)
        {
            Idle();
        }
    }
}
