using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip walkClip;
    public AudioClip alertClip;

    public EnemyDetection detection;

    private bool isPlayingWalk = false;

    void Update()
    {
        if (audioSource == null || detection == null) return;

        // ANDANDO
        if (detection.IsPatrolling)
        {
            if (!isPlayingWalk)
            {
                audioSource.clip = walkClip;
                audioSource.loop = true;
                audioSource.Play();
                isPlayingWalk = true;
            }
        }
        else
        {
            if (isPlayingWalk)
            {
                audioSource.Stop();
                isPlayingWalk = false;
            }
        }

        // ALERTA (toca uma vez)
        if (detection.IsAlert && !audioSource.isPlaying)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(alertClip);
        }
    }
}