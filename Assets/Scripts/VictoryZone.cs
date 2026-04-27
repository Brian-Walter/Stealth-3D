using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    private bool venceu = false;

    private void OnTriggerEnter(Collider other)
    {
        if (venceu) return;

        if (other.CompareTag("Player"))
        {
            venceu = true;
            VictoryLoader.CarregarVitoria();
        }
    }
}