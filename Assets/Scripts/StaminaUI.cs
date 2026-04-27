using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Image staminaFill;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Cores")]
    public Color colorNormal = new Color(0.2f, 0.8f, 0.2f);   // verde
    public Color colorLow = new Color(1f, 0.5f, 0f);      // laranja
    public Color colorEmpty = new Color(0.8f, 0.1f, 0.1f);    // vermelho

    [Header("Fade")]
    public float fadeDelay = 2f;      // segundos cheio antes de sumir
    public float fadeSpeed = 2f;

    private float fadeTimer = 0f;
    private float lastStamina = 1f;

    void Update()
    {
        float percent = player.GetStaminaPercent();

        // Atualiza fill
        staminaFill.fillAmount = percent;

        // Cor dinâmica
        if (player.IsExhausted())
            staminaFill.color = colorEmpty;
        else if (percent < 0.4f)
            staminaFill.color = colorLow;
        else
            staminaFill.color = colorNormal;

        // Fade: aparece quando stamina muda, some quando cheia
        if (Mathf.Abs(percent - lastStamina) > 0.001f)
            fadeTimer = fadeDelay;

        if (percent >= 1f)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer <= 0f)
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
        }

        lastStamina = percent;
    }
}