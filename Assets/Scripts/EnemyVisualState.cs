using UnityEngine;

public class EnemyVisualState : MonoBehaviour
{
    [Header("Referências")]
    public EnemyDetection enemyDetection;
    public Renderer npcRenderer;

    [Header("Cores dos estados")]
    public Color patrolColor = Color.gray;
    public Color alertColor = Color.red;
    public Color investigateColor = Color.yellow;

    private Material npcMaterial;
    private Color currentColor;

    void Start()
    {
        if (npcRenderer != null)
        {
            npcMaterial = npcRenderer.material; // cria instância individual por NPC
            currentColor = patrolColor;
            npcMaterial.color = currentColor;
        }
    }

    void Update()
    {
        UpdateVisualState();
    }

    void UpdateVisualState()
    {
        if (enemyDetection == null || npcMaterial == null) return;

        Color targetColor;

        if (enemyDetection.IsAlert)
            targetColor = alertColor;
        else if (enemyDetection.IsInvestigating)
            targetColor = investigateColor;
        else
            targetColor = patrolColor;

        if (currentColor != targetColor)
        {
            currentColor = targetColor;
            npcMaterial.color = currentColor;
        }
    }

    void OnDestroy()
    {
        if (npcMaterial != null)
            Destroy(npcMaterial);
    }
}