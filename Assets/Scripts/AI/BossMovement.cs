/*
Requires NavMeshAgent, Collider, Rigidbody on Boss
Bake walkable area for NavMesh
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BossMovement : MonoBehaviour
{
    [SerializeField] private Material niceMaterial;
    [SerializeField] private GameObject ghostModel;
    [SerializeField] private int bossHits = 5;
    public float walkSpeed = 0.5f;
    private float teleportingSpeed = 0f;
    public float detectionRadius = 5f;
    public float walkDuration = 2f;
    public float stopDuration = 4f;
    private float teleportingRadius = 7f;

    private NavMeshAgent agent;
    private Transform player;
    private float timer = 0f;
    private bool isWalking = true;
    private bool isTeleporting = false;
    
    public float edgeDetectionDistance = 0.5f;
    public BallColor ghostColor;
    public Room roomReference;
    public TutorialRoom tutRoomReference;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled && agent.hasPath)
        {
            Vector3 currentPos = agent.transform.position;
            NavMeshHit hit;
            if (!NavMesh.Raycast(currentPos, agent.destination, out hit, NavMesh.AllAreas))
            {
                float distanceToEdge = Vector3.Distance(currentPos, hit.position);
                if (distanceToEdge < edgeDetectionDistance)
                {
                    HandleEdgeDetected();
                }
            }
        }
        
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isTeleporting = true;
            agent.speed = teleportingSpeed;
        }
        else
        {
            isTeleporting = false;
            agent.speed = walkSpeed;
        }

        if (isTeleporting)
        {
            Vector3 directionToPlayer = player.position - transform.position; 
            directionToPlayer.Normalize();
            Vector3 diagonalDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z).normalized;
            Vector3 telePosition = player.position + diagonalDirection * teleportingRadius;
            NavMeshHit  hit;
            NavMesh.SamplePosition(telePosition, out hit, teleportingRadius, NavMesh.AllAreas);
            Vector3 finalPosition = hit.position;
            agent.transform.position = finalPosition;
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
                        Vector3 randomDirection = Random.insideUnitSphere * 2f;
                        randomDirection += transform.position;
                        NavMeshHit hit;
                        NavMesh.SamplePosition(randomDirection, out hit, 2f, NavMesh.AllAreas);
                        Vector3 finalPosition = hit.position;
                        agent.SetDestination(finalPosition);
                    }
                }
            }
            else
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

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Wall"))
    //     {
    //         HandleWallCollision(collision);
    //     }
    // }

    void HandleEdgeDetected()
    {
        Vector3 directionToPlayer = player.position - transform.position; 
        directionToPlayer.Normalize();
        Vector3 diagonalDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z).normalized;
        Vector3 telePosition = player.position + diagonalDirection * teleportingRadius;
        NavMeshHit  hit;
        NavMesh.SamplePosition(telePosition, out hit, teleportingRadius, NavMesh.AllAreas);
        Vector3 finalPosition = hit.position;
        agent.transform.position = finalPosition;
        agent.speed = teleportingSpeed;
        Debug.Log("Reached edge of NavMesh");
    }

    public void GhostHit()
    {
        bossHits--;
        if (bossHits <= 0)
        {
            ghostModel.GetComponent<SkinnedMeshRenderer>().material = niceMaterial;
            if (tutRoomReference != null)
                tutRoomReference.BossDone();
        }
        
    }
}
