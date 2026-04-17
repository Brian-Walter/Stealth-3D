using UnityEngine;

public class PlayerMovimentation : MonoBehaviour
{
    [Header("Movimento")]
    public float walkSpeed = 6f;        // Velocidade andando
    public float runSpeed = 12f;        // Velocidade correndo
    private float currentSpeed;         // Velocidade atual

    [Header("Configurações de Stamina")]
    public float staminaAtual = 100f;
    public float staminaMaxima = 100f;
    public float taxaConsumo = 20f;      // Quanto gasta por segundo
    public float taxaRegeneracao = 15f;  // Quanto ganha por segundo


    [Header("Estado do Jogador")]
    public bool estaCorrendo = false;
    private bool exausto = false; // Impede correr de novo imediatamente ao zerar

    [Header("Configurações de Delay")]
    public float tempoEsperaRegen = 2f; // Tempo em segundos para começar a recuperar
    private float cronometroRegen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // --- Movimento WASD / Seras ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = transform.right * h + transform.forward * v;
        transform.position += direction * currentSpeed * Time.deltaTime;

        // Verifica se o Shift está pressionado
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // 1. Verifica entrada do usuário e se ele NÃO está exausto
        if (Input.GetKey(KeyCode.LeftShift) && staminaAtual > 0 && !exausto)
        {
            estaCorrendo = true;
        }
        else
        {
            estaCorrendo = false;
        }

        void ConsumirStamina(float quantidade)
        {
            staminaAtual -= quantidade;
            staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaxima);
        }

        void RegenerarStamina(float quantidade)
        {
            staminaAtual += quantidade;
            staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaxima);
        }

        // 2. Lógica de Consumo
        if (estaCorrendo)
        {
            ConsumirStamina(taxaConsumo * Time.deltaTime);
            cronometroRegen = 0; // Reseta o tempo de espera sempre que gasta
        }
        // 3. Lógica de Regeneração com Delay
        else if (staminaAtual < staminaMaxima)
        {
            cronometroRegen += Time.deltaTime; // Incrementa o cronômetro

            if (cronometroRegen >= tempoEsperaRegen)
            {
                RegenerarStamina(taxaRegeneracao * Time.deltaTime);
            }
        }

        // 4. Sistema de Exaustão (Opcional, mas recomendado)
        // Se a stamina zerar, o boneco para e só volta a correr quando recuperar um pouco (ex: 10%)
        if (staminaAtual <= 0)
        {
            exausto = true;
            estaCorrendo = false;
        }

        if (exausto && staminaAtual >= 10f) // Só para de estar exausto após recuperar 10 pontos
        {
            exausto = false;
        }

    }
}
