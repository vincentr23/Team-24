using System.Collections;
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
    private GameObject player;
    private int currentNodeIndex;
    private Animator animator;
    public float deviationDistance = 5f;    // Distance the monster can deviate from the path
    public float deviationTime = 3f;        // Time the monster will wander off before returning
    public float lookAroundDuration = 2f;    // Time to look around after reaching a node
    public float loseSightDuration = 10f; // Time in seconds before giving up the chase
    private float loseSightTime; // Tracks time since player was last in sight
    private bool isChasingPlayer = false;
    public float headTurnSpeed = 2f;
    public float attackRange = 3f; // Range within which the monster can attack
    public float attackCooldown = 2f; // Cooldown between attacks
    private bool canAttack = true;
    public Collider handCollider;
    public Collider armCollider;
    public float detectionRange = 10f; // Range within which the monster detects players
    public float turnChance = 0.3f;
    [Header("Stun stuff")]
    [SerializeField] int stunned = 0;
    [SerializeField] int stunTimer = 200;


    private Quaternion targetRotation; // Target rotation when turning
    private bool isTurning = false;
     public float rotationSpeed = 5f;
    private GameObject[] playersInGame;

    void Awake()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        armCollider = GetComponent<Collider>();
        handCollider = GetComponent<Collider>();

        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Start patrol at a random node
        currentNodeIndex = UnityEngine.Random.Range(0, patrolNodes.Length);
        GoToNextNode();

        // Start patrolling at walking speed
        agent.speed = patrolSpeed;

    }

    void Update()
    {
        if (stunned > 0)
        {
            agent.speed = 0;
            return; // Stop all logic if stunned
        }
        else
        {
            agent.speed = patrolSpeed;
        }

        GetPlayersInGame();

        // Detect and possibly turn towards a nearby player
        DetectNearbyPlayers();

        if (!isTurning) // Only continue if the monster is not currently turning
        {
            if (!isChasingPlayer)
            {
                if (CanSeePlayer())
                {
                    ChasePlayer();
                }
                Patrol();
            }

            if (isChasingPlayer)
            {
                if (player != null && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                {
                    if (canAttack)
                    {
                        AttackPlayer();
                    }
                }

                if (!CanSeePlayer() && loseSightTime <= loseSightDuration)
                {
                    ChasePlayer();
                    loseSightTime += Time.deltaTime;
                }

                if (loseSightTime > loseSightDuration)
                {
                    isChasingPlayer = false;
                    loseSightTime = 0;
                }
            }
        }

        UpdateAnimationBasedOnSpeed();
    }

    private void DetectNearbyPlayers()
    {
        foreach (GameObject potentialPlayer in playersInGame)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, potentialPlayer.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Roll a random chance to see if the monster should turn towards this player
                if (Random.value <= turnChance)
                {
                    // Set the target player to turn towards
                    player = potentialPlayer;

                    // Calculate the target rotation to face the player
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                    targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

                    // Start the turning coroutine
                    StartCoroutine(TurnTowardsPlayer());

                    break; // Only handle the first player found in range
                }
            }
        }
    }

    private IEnumerator TurnTowardsPlayer()
    {
        isTurning = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            // Rotate smoothly towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the monster can see the player while turning
            if (CanSeePlayer())
            {
                // If the player is visible during the turn, start chasing
                isChasingPlayer = true;
                break;
            }

            yield return null; // Wait for the next frame
        }

        // Turn is complete
        isTurning = false;
    }
    void FixedUpdate()
    {
        if (stunned > 0)
            stunned--;
    }


    private void UpdateAnimationBasedOnSpeed()
    {
        // Get the monster's speed from the NavMeshAgent
        float speed = agent.velocity.magnitude;

        // If the monster is moving
        if (speed > 0.1f)
        {
            // Patrol speed (when not chasing) or chasing logic
            if (speed < 8f) // Patrol Speed (adjust this if necessary)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            else // Running or Chasing (speed > 8f, adjust as needed)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
        }
        else // If the monster is not moving
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }


    // Moves the agent to the next patrol node
    private void GoToNextNode()
    {

        int randomNodeIndex;

        do
        {
            randomNodeIndex = Random.Range(0, patrolNodes.Length);
        } while (randomNodeIndex == currentNodeIndex);

        currentNodeIndex = randomNodeIndex;

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
        agent.SetDestination(player.transform.position);
        isChasingPlayer = true;
    }

    // Checks if the player is within sight and field of view
    public bool CanSeePlayer()
    {
        if (!isChasingPlayer)
        {
            if (playersInGame != null) {
                // Check distance to the player
                foreach (GameObject p in playersInGame)
                {
                    if (Vector3.Distance(transform.position, p.transform.position) < chaseDistance)
                    {
                        // Check if the player is within the field of view
                        Vector3 directionToPlayer = (p.transform.position - transform.position).normalized;
                        float angle = Vector3.Angle(transform.forward, directionToPlayer);

                        // Player is within the field of view
                        if (angle < fieldOfViewAngle / 2)
                        {
                            // Check if there is a clear line of sight to the player
                            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, chaseDistance))
                            {
                                if (hit.transform.CompareTag("Player")) // Ensure the hit object is the player
                                {
                                    player = p;
                                    return true; // Player is in sight
                                }
                            }
                        }
                    }
                    locataionEffectorObject.transform.position = locataionEffectorObjectOrig.transform.position;
                    return false; // Player not in sight
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
            {
                // Check if the player is within the field of view
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                Debug.Log("Found player");
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
        return false;

    }

     void GetPlayersInGame()
    {
        // Get all active players in the scene (ensure your player objects have a "Player" script attached)
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
    }

    private void AttackPlayer()
    {
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("isAttacking");
        }

        armCollider.enabled = true;
        handCollider.enabled = true;

        // Start cooldown
        StartCoroutine(AttackCooldown());
    }

    // Coroutine to handle cooldown
    private System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    private void OnTriggerEnter(Collider other)

    {
        // Check if the object hit has the Player tag
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().IsHit();
                Debug.Log("Player hit by monster!");
            }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Monster collided with the player!");
        // Check if the player touched the monster
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            ChasePlayer();
        }
    }

    public void Stun()
    {
        stunned = stunTimer;
    }
}


