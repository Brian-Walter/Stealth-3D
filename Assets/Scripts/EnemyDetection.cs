using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Referências")]
    public EnemyVision enemyVision;
    public EnemyPatrol enemyPatrol;
    public Transform target;

    [Header("Configuração")]
    public float rotationSpeed = 5f;
    public float investigateTime = 3f;

    // Estados legíveis por outros scripts
    public bool IsAlert => isAlert && !isInvestigating;
    public bool IsInvestigating => isInvestigating;
    public bool IsPatrolling => !isAlert && !isInvestigating;

    [SerializeField] private bool isAlert = false;

    private bool wasAlert = false;
    private float investigateCounter = 0f;
    private bool isInvestigating = false;
    private Vector3 lastKnownPosition;

    void Update()
    {
        if (enemyVision == null) return;

        if (enemyVision.PlayerInSight && target != null)
        {
            lastKnownPosition = target.position;
            isInvestigating = false;
            investigateCounter = 0f;
            EnterAlertState();
        }
        else if (isAlert)
        {
            ExitAlertState();
        }
    }

    void EnterAlertState()
    {
        if (!wasAlert)
        {
            wasAlert = true;
            isAlert = true;

            if (enemyPatrol != null)
                enemyPatrol.enabled = false;

            Debug.Log("NPC em alerta!");
        }

        LookAtTarget(lastKnownPosition);
    }

    void ExitAlertState()
    {
        if (!isInvestigating)
        {
            isInvestigating = true;
            investigateCounter = investigateTime;
            Debug.Log("NPC investigando...");
        }

        LookAtTarget(lastKnownPosition);

        investigateCounter -= Time.deltaTime;

        if (investigateCounter <= 0f)
        {
            isAlert = false;
            wasAlert = false;
            isInvestigating = false;

            if (enemyPatrol != null)
                enemyPatrol.enabled = true;

            Debug.Log("NPC retomando patrulha.");
        }
    }

    void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 lookDirection = targetPosition - transform.position;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}