using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolNodes;         // Array of patrol nodes
    public float chaseDistance = 10f;       // Distance to start chasing the player
    public float fieldOfViewAngle = 110f;   // Angle for field of view
    public float patrolSpeed = 3f;          // Speed when patrolling (walking)
    public float chaseSpeed = 6f;           // Speed when chasing the player (running)
    private NavMeshAgent agent;
    private Transform player;
    private int currentNodeIndex;
    private Animator animator;
    private bool isWandering;
    public float deviationDistance = 5f;     // Distance the monster can deviate from the path
    public float deviationTime = 3f;        // Time the monster will wander off before returning

    void Awake()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Start patrol at a random node
        currentNodeIndex = UnityEngine.Random.Range(0, patrolNodes.Length);
        GoToNextNode();

        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Start patrolling at walking speed
        agent.speed = patrolSpeed;
    }

    void Update()
    {
        // Check if player is within sight and range
        if (player != null && CanSeePlayer())
        {
            // Chase the player if detected
            ChasePlayer();
        }
        else
        {
            // Patrol if the player is not in sight
            Patrol();
        }

        // Set animations based on agent's movement speed
        HandleAnimationsBasedOnSpeed();
    }

    // Updates the animations based on the agent's movement speed
    private void HandleAnimationsBasedOnSpeed()
    {
        float agentSpeed = agent.velocity.magnitude;

        // If the monster is moving
        if (agentSpeed > 0.1f)
        {
            if (agentSpeed > patrolSpeed * 1.5f) // Running if speed is higher than patrolSpeed threshold
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
            else // Walking if speed is lower
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
        }
        else // If the monster is idle
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
    }

    // Moves the agent to the next patrol node
    private void GoToNextNode()
    {
        if (patrolNodes.Length == 0) return; // No nodes to patrol

        // Set the destination to the current node
        agent.destination = patrolNodes[currentNodeIndex].position;

        // Move to the next node in a loop
        currentNodeIndex = (currentNodeIndex + 1) % patrolNodes.Length;

        // Ensure the agent is walking
        agent.speed = patrolSpeed;
    }

    // Patrols the area by moving from node to node
    private void Patrol()
    {
        // Check if the agent has reached the node
        if (agent.remainingDistance < 0.5f)
        {
            if (!isWandering)
            {
                // Start wandering away from the path
                StartCoroutine(Wander());
            }
            else
            {
                GoToNextNode(); // Move to the next node if not wandering
            }
        }
    }

    private IEnumerator Wander()
    {
        isWandering = true;

        // Random point to wander to
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * deviationDistance;
        randomDirection += transform.position; // Offset from the current position
        NavMeshHit hit;

        // Find a valid point on the NavMesh
        NavMesh.SamplePosition(randomDirection, out hit, deviationDistance, NavMesh.AllAreas);
        agent.SetDestination(hit.position); // Set the destination to the random point

        // Wait for the specified duration of wandering
        yield return new WaitForSeconds(deviationTime);

        // Return to the current patrol node
        GoToNextNode();
        isWandering = false;
    }

    // Chases the player when in range
    private void ChasePlayer()
    {
        // Set speed to running speed and chase the player
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    // Checks if the player is within sight and field of view
    private bool CanSeePlayer()
    {
        // Check distance to the player
        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            // Check if the player is within the field of view
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            // Player is within the field of view
            if (angle < fieldOfViewAngle / 2)
            {
                // Check if there is a clear line of sight to the player
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, chaseDistance))
                {
                    if (hit.transform.CompareTag("Player")) // Ensure the hit object is the player
                    {
                        return true; // Player is in sight
                    }
                }
            }
        }
        return false; // Player not in sight
    }
}
