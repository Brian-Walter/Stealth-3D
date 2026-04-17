using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;
    public EnemyPatrol patrol;
    public EnemyDetection detection;

    void Update()
    {
        if (animator == null) return;

        bool isWalking = false;

        if (patrol != null && patrol.enabled)
        {
            isWalking = true;
        }

        if (detection != null && detection.IsAlert)
        {
            isWalking = false;
        }

        animator.SetBool("isWalking", isWalking);
    }
}