using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movementSpeed = 500f;
    [SerializeField] float enemyFollowRotationSpeedFactor = 20;
    //[SerializeField] float maxForceSpeed = 1f;
    //[SerializeField] float maxTorqueSpeed = 1f;
    [SerializeField] int enemyFollowWiggleRoom;

    [SerializeField] GameObject sword;
    //[SerializeField] SliderJoint2D swordThrust;

    //TODO: Add a stamina bar
    [SerializeField] float maxAttackSpeed = 100;
    [SerializeField] float attackPower = 1000;

    //[SerializeField] TextMeshProUGUI thrustPowerText;

    [SerializeField] GameObject player;

    //Camera cam;
    Rigidbody2D rb;

    FixedJoint2D swordIdle;
    SliderJoint2D swordThrust;
    HingeJoint2D swordSwing;

    //Vector2 mouseWorldPos;

    Vector2 forceDirection = new Vector2(0, 0);
    Vector2 playerDirection = new Vector2(0, 0);

    float playerDistance;

    JointMotor2D swordMotor;
    JointAngleLimits2D swordSwingLimits;

    float enemyPlayerAngle;

    float attackSpeed = 0;

    bool charging = false;

    [SerializeField] int attackState = 1;

    private void CalcPlayerDir()
    {
        playerDirection.x = player.transform.position.x - transform.position.x;
        playerDirection.y = player.transform.position.y - transform.position.y;
    }
    private void CalcPlayerDist()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);
        //Debug.Log(playerDistance);
    }
    private void CalcAttackState()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            attackState = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            attackState = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            attackState = 3;
        }
    }

    private void Move()
    {
        //Using the unity input system
        //forceDirection.x = Input.GetAxis("Horizontal");
        //forceDirection.y = Input.GetAxis("Vertical");
        CalcPlayerDir();

        forceDirection = playerDirection.normalized;
        rb.AddForce(forceDirection * movementSpeed);
    }

    private void FacePlayer()
    {
        //mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        CalcPlayerDir();

        //float mousePlayerAngleOffset = Vector2.Distance((Vector2)transform.up.normalized, mouseDirection.normalized);

        //float mousePlayerAngle = Vector2.Angle((Vector2)transform.up, mouseDirection);
        //Signed angle returns the angle between -180 and 180 instead of an absolute value like Vector2.Angle
        enemyPlayerAngle = Vector2.SignedAngle((Vector2)transform.up, playerDirection);

        //Debug.Log(mousePlayerAngle);

        //Rotate
        if (enemyPlayerAngle > enemyFollowWiggleRoom)
        {
            rb.AddTorque(enemyFollowRotationSpeedFactor * Mathf.Abs(enemyPlayerAngle));
        }
        if (enemyPlayerAngle < -enemyFollowWiggleRoom)
        {
            rb.AddTorque(-enemyFollowRotationSpeedFactor * Mathf.Abs(enemyPlayerAngle));
        }
    }

    private void AddAttackSpeed()
    {
        if (attackSpeed < maxAttackSpeed)
        {
            attackSpeed += 1f;
        }

        //Debug.Log(attackSpeed);
        //thrustPowerText.text = attackSpeed.ToString();
    }

    private void ChargeThrust()
    {
        charging = true;

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
        charging = false;
        //thrustPowerText.text = attackSpeed.ToString();
    }

    private void ChargeSwingLeft()
    {
        /*
        swordSwingLimits = swordSwing.limits;
        swordSwingLimits.min = 330; //315
        swordSwingLimits.max = 450;
        swordSwing.limits = swordSwingLimits;
        */

        charging = true;

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
        charging = false;
        //thrustPowerText.text = attackSpeed.ToString();
    }

    private void ChargeSwingRight()
    {
        /*
        swordSwingLimits = swordSwing.limits;
        swordSwingLimits.min = 270;
        swordSwingLimits.max = 390; //405
        swordSwing.limits = swordSwingLimits;
        */

        charging = true;

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
        charging = false;
        //thrustPowerText.text = attackSpeed.ToString();
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
        //cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        swordIdle = sword.GetComponent<FixedJoint2D>();
        swordThrust = sword.GetComponent<SliderJoint2D>();
        swordSwing = sword.GetComponent<HingeJoint2D>();

        //swordIdle.enabled = false;
        swordThrust.enabled = false;
        swordSwing.enabled = false;

        //thrustPowerText.text = attackSpeed.ToString();
    }

    // FixedUpdate is used to keep the commands in sync with the physics engine
    void FixedUpdate()
    {
        CalcPlayerDist();

        if (playerDistance > 6 & playerDistance < 18)
        {
            Move();
        }
        
        FacePlayer();
    }

    void Update()
    {
        CalcPlayerDist();
        CalcAttackState();

        if (attackSpeed > 90 & playerDistance < 6)
        {
            if (attackState == 1)
            {
                Thrust();
            }
            else if (attackState == 2)
            {
                SwingLeft();
            }
            else if (attackState == 3)
            {
                SwingRight();
            }
        }
        else if (playerDistance > 6 & playerDistance < 10)
        {
            if (attackState == 1)
            {
                ChargeThrust();
            }
            else if (attackState == 2)
            {
                ChargeSwingLeft();
            }
            else if (attackState == 3)
            {
                ChargeSwingRight();
            }
        }
        else if (playerDistance > 10)
        {
            Idle();
        }

        if (charging)
        {
            AddAttackSpeed();
            //Debug.Log(attackSpeed);
        }
    }
}
