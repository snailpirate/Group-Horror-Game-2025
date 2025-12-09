using UnityEngine;
using UnityEngine.AI; // Required for NavMesh

public class TeddyBearMovement : MonoBehaviour
{
    [Header("Settings")]
    public float fleeRadius = 10f;  // How close player must be to scare bear
    public float fleeDistance = 5f; // How far bear runs
    public float speed = 3.5f;

    [Header("Stuck Prevention")]
    public float stuckCheckTime = 0.5f; // How often to check if stuck
    public float minVelocity = 0.5f;    // Minimum speed to be considered "moving"

    private NavMeshAgent agent;
    private Transform player;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Find player automatically
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. Is the player close enough to scare me?
        if (distanceToPlayer < fleeRadius)
        {
            RunAway();
            CheckIfStuck();
        }
    }

    void RunAway()
    {
        // Don't calculate a new path every single frame (saves performance)
        if (agent.pathPending) return;

        // Calculate the direction AWAY from the player
        Vector3 dirToPlayer = transform.position - player.position;
        Vector3 newPos = transform.position + dirToPlayer.normalized * fleeDistance;

        // NAVMESH MAGIC: Find the nearest valid point on the floor close to that target
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPos, out hit, 2.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    // THE FIX: This function detects if we are cornered
    void CheckIfStuck()
    {
        timer += Time.deltaTime;
        if (timer > stuckCheckTime)
        {
            timer = 0;

            // If we are supposed to be running, but our speed is basically zero...
            // WE ARE STUCK IN A CORNER!
            if (agent.velocity.magnitude < minVelocity)
            {
                PanicMove();
            }
        }
    }

    void PanicMove()
    {
        // Pick a random point anywhere in a 5-unit radius to "break" the corner lock
        Vector3 randomDirection = Random.insideUnitSphere * 5f;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    // Optional: Draw gizmos to see the "Fear Zone" in Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }
}