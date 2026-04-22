using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    public EnemyDetection detection;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no objeto!");
            return;
        }
    }

    void Update()
    {
        if (animator == null || detection == null) return;

        animator.SetBool("isWalking", detection.IsPatrolling);
        animator.SetBool("isAlert", detection.IsAlert || detection.IsInvestigating);
    }
}