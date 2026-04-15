using UnityEngine;
// Descomente a linha abaixo quando for integrar o NavMesh
// using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Configuração de Patrulha")]
    public Transform[] waypoints;
    public float speed = 3f;
    public float waitTime = 2f;

    private int currentWaypoint = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    // Descomente quando for usar NavMeshAgent:
    // private NavMeshAgent agent;

    void Start()
    {
        // Descomente quando for usar NavMeshAgent:
        // agent = GetComponent<NavMeshAgent>();
        // agent.speed = speed;
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
            }
            return;
        }

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        Transform target = waypoints[currentWaypoint];

        // --- Versão para cena de testes (sem NavMesh) ---
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // --- Versão com NavMeshAgent (para o projeto final) ---
        // agent.SetDestination(target.position);
        // if (agent.remainingDistance <= agent.stoppingDistance) { ... }

        // Chegou no waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    void NextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    // Desenha os waypoints no Editor para facilitar o posicionamento
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