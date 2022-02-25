using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoManagement : MonoBehaviour
{
    [SerializeField]
    private Transform extendedIntakePosition;
    [SerializeField]
    private Transform retractedIntakePosition;
    [SerializeField]
    private GameObject intake;

    [SerializeField]
    private GameObject cargoPosition1;
    [SerializeField]
    private GameObject cargoPosition2;
    [SerializeField]
    private GameObject shootingPosition;

    [SerializeField]
    private GameObject hubTarget;
    [SerializeField]
    [Tooltip("This constant will be multiplied by the distance towards the target when shooting.")]
    private float shootingDistanceMultiplier;
    [SerializeField]
    [Tooltip("This will be the y velocity of the ball when shooting.")]
    private float shootingYVelocity;

    [SerializeField]
    [Tooltip("The minimum amount of seconds between two shots.")]
    private float shootingCooldown;

    //The amount of cargos currently inside the robot.

    //True when intake is retracted.
    private bool isRetracted = true;

    private List<GameObject> balls;

    private float lastShotTime;

    private void Start()
    {
        balls = new List<GameObject>();
        lastShotTime = Time.time;
    }

    private void Update()
    {
        CheckInputs();
        Debug.Log(balls.Count);
        KeepCargoInPlace();
    }

    private void CheckInputs()
    {
        if (Input.GetButtonDown("X"))
        {
            MoveIntake(!isRetracted);
        }
        if(Input.GetButton("A") && Time.time - lastShotTime > shootingCooldown)
        {
            ShootBall(balls[0]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "red_cargo" || other.tag =="blue_cargo") && Input.GetButton("RB"))
        {
            IntakeBall(other.gameObject);
        }
    }

    private void MoveIntake(bool retract)
    {
        if (retract)
        {
            intake.transform.SetPositionAndRotation(retractedIntakePosition.position, retractedIntakePosition.rotation);
            isRetracted = true;
        }
        else
        {
            intake.transform.SetPositionAndRotation(extendedIntakePosition.position, extendedIntakePosition.rotation);
            isRetracted = false;
        }
    }

    private void IntakeBall(GameObject ball)
    {
        if(balls.Count >= 2)
        {
            return;
        }
        if(ball.transform.parent == transform)
        {
            return;
        }
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        // ball.GetComponent<Rigidbody>().isKinematic = true;

        ball.transform.parent = transform;

        if (balls.Count == 0)
        {
            ball.transform.position = cargoPosition1.transform.position;
        } else if (balls.Count == 1)
        {
            ball.transform.position = cargoPosition2.transform.position;
        }

        balls.Add(ball);

    }

    private void KeepCargoInPlace()
    {
        if(balls.Count == 1)
        {
            balls[0].transform.position = cargoPosition1.transform.position;
            balls[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
            balls[0].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }else if(balls.Count == 2)
        {
            balls[0].transform.position = cargoPosition1.transform.position;
            balls[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
            balls[0].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            balls[1].transform.position = cargoPosition2.transform.position;
            balls[1].GetComponent<Rigidbody>().velocity = Vector3.zero;
            balls[1].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void ShootBall(GameObject ball)
    {
        ball.transform.position = shootingPosition.transform.position;
        float distance = Vector3.Distance(ball.transform.position, hubTarget.transform.position);

        Vector3 velocity = (hubTarget.transform.position - ball.transform.position)*shootingDistanceMultiplier;
        velocity.y = shootingYVelocity;

        ball.transform.parent = null;

        //ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().velocity = velocity;

        balls.Remove(ball);
        Debug.Log(balls.Count);
        if(balls.Count == 1)
        {
            balls[0].transform.position = cargoPosition1.transform.position;
        }

        lastShotTime = Time.time;

    }

}
