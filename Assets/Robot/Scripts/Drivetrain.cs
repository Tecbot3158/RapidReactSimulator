using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drivetrain : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The maximum speed that the robot will have when moving forward with high gear")]
    private float maxHighGearForwardSpeed;

    [SerializeField]
    [Tooltip("The maximum speed that the robot will have when moving forward with low gear")]
    private float maxLowGearForwardSpeed;

    [SerializeField]
    [Tooltip("The maximum speed that the robot will have when moving forward with high gear")]
    private float maxHighGearTurnSpeed;

    [SerializeField]
    [Tooltip("The maximum speed that the robot will have when moving forward with low gear")]
    private float maxLowGearTurnSpeed;

     [SerializeField]
    [Tooltip("The maximum speed that the robot will have when moving to the sides")]
    private float maxXSpeed;

    private Rigidbody robot;
    private bool highGear = false;

    public enum DrivingMode {Tank, Swerve, Mecanum};
    private DrivingMode currentDrivingMode;

    void Start()
    {
        robot = GetComponent<Rigidbody>();
        currentDrivingMode = DrivingMode.Tank;
    }

    void FixedUpdate()
    {
        switch (currentDrivingMode)
        {
            case DrivingMode.Tank:
                TankDrive();
                break;
            case DrivingMode.Mecanum:
                MecanumDrive();
                break;
            case DrivingMode.Swerve:
                SwerveDrive();
                break;
            default:
                Debug.LogError("Driving mode not recognized");
                break;
        }

        CheckInputs();
    }

    public void ChangeDrivingMode(DrivingMode mode)
    {
        currentDrivingMode = mode;
    }

    void CheckInputs()
    {
        /*
         * Mapping:
         * 
         * POV
         * Up: tank
         * Left: swerve
         * Right: mecanum
         */

        if (Input.GetAxisRaw("POVHorizontal") > 0.3)
        {
            ChangeDrivingMode(DrivingMode.Mecanum);
        }
        if (Input.GetAxisRaw("POVHorizontal") < -0.3)
        {
            ChangeDrivingMode(DrivingMode.Swerve);
        }
        if (Input.GetAxisRaw("POVVertical") > 0.3)
        {
            ChangeDrivingMode(DrivingMode.Tank);
        }

        highGear = Input.GetButton("LB");

    }

    void TankDrive()
    {
        float speed = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        float upSpeed = robot.velocity.y; 

        Vector3 direction = transform.right;

        if (highGear)
        {
            //robot.AddForce(direction * speed * maxHighGearForwardSpeed * Time.deltaTime);
            Vector3 velocity = direction * speed * maxHighGearForwardSpeed * Time.deltaTime;
            velocity.y = upSpeed;

            robot.velocity = velocity; 
            robot.angularVelocity = transform.up * maxHighGearTurnSpeed * turn * Time.deltaTime;
            print(robot.velocity);
        }
        else
        {
            //robot.AddForce(direction * speed * maxLowGearForwardSpeed * Time.deltaTime);
            Vector3 velocity = (direction * speed * maxLowGearForwardSpeed * Time.deltaTime);
            velocity.y = upSpeed;

            robot.velocity = velocity;
            robot.angularVelocity = transform.up * maxLowGearTurnSpeed * turn * Time.deltaTime;
        }

    }

    void MecanumDrive()
    {
        float ySpeed = Input.GetAxis("Vertical");
        float xSpeed = Input.GetAxis("Horizontal");
        float turn = Input.GetAxis("RightXAxis");
        

        Vector3 direction = transform.right;
        Vector3 perpendicular = -transform.forward;

        Vector3 perpendicularVelocity = (perpendicular * xSpeed * maxXSpeed * Time.deltaTime);
        if (highGear)
        {
            //robot.AddForce(direction * ySpeed * maxHighGearForwardSpeed * Time.deltaTime);
            robot.velocity = (direction * ySpeed * maxHighGearForwardSpeed * Time.deltaTime)+perpendicularVelocity;
            robot.angularVelocity = transform.up * maxHighGearTurnSpeed * turn * Time.deltaTime;
        }
        else
        {
            //robot.AddForce(direction * ySpeed * maxLowGearForwardSpeed * Time.deltaTime);
            robot.velocity = (direction * ySpeed * maxLowGearForwardSpeed * Time.deltaTime)+perpendicularVelocity;
            robot.angularVelocity = transform.up * maxLowGearForwardSpeed * turn * Time.deltaTime;
        }

    }

    void SwerveDrive()
    {
        float ySpeed = Input.GetAxis("Vertical");
        float xSpeed = Input.GetAxis("Horizontal");
        float turn = Input.GetAxis("RightXAxis");

        robot.velocity = (Vector3.right * ySpeed * maxXSpeed * Time.deltaTime) 
            - (Vector3.forward * xSpeed * maxXSpeed * Time.deltaTime);

        if (highGear)
        {
            robot.angularVelocity = transform.up * maxHighGearTurnSpeed * turn * Time.deltaTime;
        }
        else
        {
            robot.angularVelocity = transform.up * maxLowGearForwardSpeed * turn * Time.deltaTime;
        }

    }
}
