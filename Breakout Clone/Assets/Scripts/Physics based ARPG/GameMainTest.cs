using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainTest : MonoBehaviour
{
    [SerializeField] HingeJoint2D hingeComponent;
    [SerializeField] Transform obj1;
    [SerializeField] Transform obj2;

    JointMotor2D motorObj = new JointMotor2D();

    int speed;

    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.gravity = new Vector2(0, 0);
        speed = 2000;
        motorObj.maxMotorTorque = 100000;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            hingeComponent.useMotor = true;
            motorObj.motorSpeed = speed;
            hingeComponent.motor = motorObj;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            //hingeComponent.useMotor = false;
            motorObj.motorSpeed = 0;
            hingeComponent.motor = motorObj;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            hingeComponent.useMotor = true;
            motorObj.motorSpeed = speed * -1;
            hingeComponent.motor = motorObj;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            //hingeComponent.useMotor = false;
            motorObj.motorSpeed = 0;
            hingeComponent.motor = motorObj;
        }

        //Debug.Log(hingeComponent.jointAngle);
        //Debug.Log(hingeComponent.jointSpeed);
        //Debug.Log(Vector2.Angle((Vector2) obj1.position, (Vector2) obj2.position));
    }
}
