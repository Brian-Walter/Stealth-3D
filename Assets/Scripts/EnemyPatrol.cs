using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Configuração de Patrulha")]
    public Transform[] waypoints;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    public bool IsMoving => !isWaiting && waypoints.Length > 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWaiting = false;
                NextWaypoint();
                agent.SetDestination(waypoints[currentWaypoint].position);
            }
            return;
        }

        // Chegou no waypoint?
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    void NextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawSphere(waypoints[i].position, 0.3f);
            int next = (i + 1) % waypoints.Length;
            if (waypoints[next] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
        }
    }
}