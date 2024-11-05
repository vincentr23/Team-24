using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolNodes;         // Array of patrol nodes
    public float chaseDistance = 10f;       // Distance to start chasing the player
    public float fieldOfViewAngle = 110f;   // Angle for field of view
    public float patrolSpeed = 3f;          // Speed when patrolling (walking)
    public float chaseSpeed = 6f;           // Speed when chasing the player (running)
    public GameObject locataionEffectorObject;
    public GameObject locataionEffectorObjectOrig;
    private NavMeshAgent agent;
    private Transform player;
    private int currentNodeIndex;
    private Animator animator;
    public float deviationDistance = 5f;    // Distance the monster can deviate from the path
    public float deviationTime = 3f;        // Time the monster will wander off before returning
    public float lookAroundDuration = 2f;    // Time to look around after reaching a node
    private bool isLookingAround = false;
    private bool isChasing = false;

    public float loseSightDuration = 10f; // Time in seconds before giving up the chase
    private float loseSightTime; // Tracks time since player was last in sight
    private bool isChasingPlayer = false;
    public float headTurnSpeed = 2f;
    public float attackRange = 2f;  // Distance within which the monster will attack
    public float attackCooldown = 1.5f; // Time between attacks
    private bool isAttacking = false;
    private float lastAttackTime;

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
        // Continuously check for the nearest player if not yet assigned
        FindNearestPlayer();

        // If no players were found, return early
        if (player == null) return;

        // Check if the player is visible
        if (CanSeePlayer() && Vector3.Distance(transform.position, player.position) > attackRange)
        {
            loseSightTime = Time.time; // Reset timer when the player is in sight
            ChasePlayer();
            //SmoothHeadTurn();
        }
        else if (isChasingPlayer && Time.time - loseSightTime < loseSightDuration)
        {
            // Continue chasing if within lose sight duration
            ChasePlayer();
            //SmoothHeadTurn();
        }
        else if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            // Stop chasing if the player is out of sight for more than 10 seconds
            isChasingPlayer = false;
            Patrol();
        }

        // Update animations based on agent's movement speed
        HandleAnimationsBasedOnSpeed();
    }

    // Updates the animations based on the agent's movement speed
    private void HandleAnimationsBasedOnSpeed()
    {
        float agentSpeed = agent.velocity.magnitude;
        Debug.Log($"Agent Speed: {agentSpeed}"); // Debug statement to check speed

        // If the monster is moving
        if (agentSpeed > 0.1f)
        {
            if (agentSpeed > patrolSpeed * 1.5f) // Running if speed is higher than patrolSpeed threshold
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false); // Set idle to true when stopped
            }
            else // Walking if speed is lower
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false); // Set idle to true when stopped
            }
        }
        else // If the monster is idle
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true); // Set idle to true when stopped

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
        bool LookAround = UnityEngine.Random.Range(0, 2) == 1;

        if (agent.remainingDistance < .5f) {
            // Check if the agent has reached the node
            GoToNextNode();
        }
    }


    // Chases the player when in range
    private void ChasePlayer()
    {
        // Set speed to running speed and chase the player
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        isChasingPlayer = true;
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
        locataionEffectorObject.transform.position = locataionEffectorObjectOrig.transform.position;
        return false; // Player not in sight
    }

    void FindNearestPlayer()
    {
        // Find all players with the "Player" tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // If no players found, set player to null and exit
        if (players.Length == 0)
        {
            player = null;
            return;
        }

        // Initialize variables to track the closest player
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        // Loop through all players to find the nearest one
        foreach (GameObject potentialPlayer in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, potentialPlayer.transform.position);

            // If this player is closer than the current closest, update the closest player
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = potentialPlayer.transform;
            }
        }

        // Set the closest player as the target
        player = closestPlayer;
    }

    private void SmoothHeadTurn()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly interpolate towards the target rotation
        locataionEffectorObject.transform.rotation = Quaternion.Slerp(
            locataionEffectorObject.transform.rotation,
            targetRotation,
            Time.deltaTime * headTurnSpeed
        );
    }

    private void AttackPlayer()
    {
        // Check if cooldown period has passed
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            agent.isStopped = true; // Stop movement

            // Trigger attack animation
            animator.SetTrigger("isAttacking");

            // Update last attack time
            lastAttackTime = Time.time;
        }
        else
        {
            // Resume chasing if attack cooldown not met
            agent.isStopped = false;
            ChasePlayer();
        }
    }

}
