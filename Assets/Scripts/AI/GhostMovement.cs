/*
Requires NavMeshAgent, Collider, Rigidbody on Ghost
Bake walkable area for NavMesh
Tag room walls with "Wall" to allow collision
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float detectionRadius = 5f;
    private float teleportingRadius = 7f;
    private float teleportingSpeed = 0f;
    public float walkDuration = 3f;
    public float stopDuration = 2f;

    private NavMeshAgent agent;
    private bool isRunningAway = false;
    private Transform player;
    private float timer = 0f;
    private bool isWalking = true;

    public BallColor ghostColor;
    public Room roomReference;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isRunningAway = true;
            agent.speed = runSpeed;
        }
        else
        {
            isRunningAway = false;
            agent.speed = walkSpeed;
        }

        if (isRunningAway)
        {
            Vector3 directionToPlayer = transform.position - player.position;
            Vector3 newPosition = transform.position + directionToPlayer.normalized;
            agent.SetDestination(newPosition);
        }
        else
        {
            if (isWalking)
            {
                if (!agent.hasPath || agent.remainingDistance < 0.5f)
                {
                    timer += Time.deltaTime;
                    if (timer >= walkDuration)
                    {
                        timer = 0f;
                        isWalking = false;
                    }
                    else
                    {
                        Vector3 randomDirection = Random.insideUnitSphere * 10f;
                        randomDirection += transform.position;
                        NavMeshHit hit;
                        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
                        Vector3 finalPosition = hit.position;
                        agent.SetDestination(finalPosition);
                    }
                }
            }
            else // If not walking, stop for the stop duration
            {
                timer += Time.deltaTime;
                if (timer >= stopDuration)
                {
                    timer = 0f;
                    isWalking = true;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
    }

    void HandleWallCollision(Collision collision)
    {
        agent.speed = teleportingSpeed;
        Vector3 directionToPlayer = player.position - transform.position; 
        directionToPlayer.Normalize();
        Vector3 diagonalDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z).normalized;
        Vector3 telePosition = player.position + diagonalDirection * teleportingRadius;
        NavMeshHit  hit;
        NavMesh.SamplePosition(telePosition, out hit, teleportingRadius, NavMesh.AllAreas);
        Vector3 finalPosition = hit.position;
        agent.transform.Rotate(0f, 0f, 90f);
        agent.transform.position = finalPosition;
    }

    public void GhostHit()
    {
        roomReference.GhostHit();
    }
}
