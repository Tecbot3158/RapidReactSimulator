using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubTrigger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The positions where the ball will be spawned to exit.")]
    private GameObject[] exits;

    [SerializeField]
    [Tooltip("When scoring to this hub, the alliance will be rewarded this amout of points.")]
    private int points;

    [SerializeField]
    private GameObject redCargoPrefab, blueCargoPrefab;

    [SerializeField]
    [Tooltip("The amount of time that it will take for a ball to exit.")]
    private float cargoExitCooldown;

    private GameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "red_cargo")
        {
            Destroy(other.gameObject);
            manager.AddToScore(points, true);
            Invoke("SpawnRedBall", cargoExitCooldown);
        }
        if (other.tag == "blue_cargo")
        {
            Destroy(other.gameObject);
            manager.AddToScore(points, false);
            Invoke("SpawnBlueBall", cargoExitCooldown);
        }
    }

    private void SpawnRedBall()
    {
        GameObject ball = Instantiate(redCargoPrefab);
        ball.transform.position = exits[Random.Range(0, exits.Length-1)].transform.position;
    }
    private void SpawnBlueBall()
    {
        GameObject ball = Instantiate(blueCargoPrefab);
        ball.transform.position = exits[Random.Range(0, exits.Length-1)].transform.position;
    }
}
