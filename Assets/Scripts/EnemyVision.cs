using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Alvo")]
    public Transform target;

    [Header("Configuração de visão")]
    public float viewDistance = 8f;
    [Range(0f, 360f)]
    public float viewAngle = 60f;

    [Header("Obstáculos")]
    public LayerMask obstacleMask;

    [Header("Debug")]
    [SerializeField] private bool playerInSight = false;

    // Leitura pública, escrita privada — outros scripts consultam, não alteram
    public bool PlayerInSight => playerInSight;

    private bool previousInSight = false;

    void Update()
    {
        CheckVision();
    }

    void CheckVision()
    {
        if (target == null)
        {
            playerInSight = false;
            return;
        }

        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0f;
        float distanceToTarget = directionToTarget.magnitude;

        // 1. Verificação de distância (mais barata — faz primeiro)
        if (distanceToTarget > viewDistance)
        {
            playerInSight = false;
            LogStateChange();
            return;
        }

        // 2. Verificação de ângulo
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget > viewAngle / 2f)
        {
            playerInSight = false;
            LogStateChange();
            return;
        }

        // 3. Verificação de obstáculos via Raycast (mais cara — faz por último)
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = (target.position - rayOrigin).normalized;
        float rayDistance = Vector3.Distance(rayOrigin, target.position);

        if (Physics.Raycast(rayOrigin, rayDirection, rayDistance, obstacleMask))
        {
            // Raycast bateu em obstáculo antes de chegar ao jogador
            playerInSight = false;
        }
        else
        {
            playerInSight = true;
        }

        LogStateChange();
    }

    // Loga apenas quando o estado muda, não todo frame
    void LogStateChange()
    {
        if (playerInSight != previousInSight)
        {
            previousInSight = playerInSight;
            if (playerInSight)
                Debug.Log("Jogador detectado!");
            else
                Debug.Log("Jogador perdido.");
        }
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward * viewDistance;

        Quaternion leftRotation = Quaternion.Euler(0f, -viewAngle / 2f, 0f);
        Quaternion rightRotation = Quaternion.Euler(0f, viewAngle / 2f, 0f);

        // Cone de visão
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, leftRotation * forward);
        Gizmos.DrawRay(origin, rightRotation * forward);

        // Raio máximo de detecção
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, viewDistance);

        // Linha até o alvo com cor por estado
        if (target != null)
        {
            Gizmos.color = playerInSight ? Color.green : Color.white;
            Gizmos.DrawLine(origin, target.position);
        }
    }
}