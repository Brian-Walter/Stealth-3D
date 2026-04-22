using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CharacterController))]
public class PlayerSettings : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidadeAndar = 5f;
    [SerializeField] private float velocidadeCorrer = 10f;
    [SerializeField] private float gravidade = -9.81f;

    [Header("Stamina")]
    [SerializeField] private float staminaMaxima = 100f;
    [SerializeField] private float staminaGastoPorSegundo = 20f;
    [SerializeField] private float staminaRecuperacaoPorSegundo = 15f;
    [SerializeField] private float delayParaRecuperar = 2f;

    [Header("UI (Opcional)")]
    [SerializeField] private Slider barraStamina;
    [SerializeField] private Image preenchimentoStamina;
    [SerializeField] private Color corStaminaCheia = new Color(0.2f, 0.85f, 0.4f);
    [SerializeField] private Color corStaminaBaixa = new Color(0.9f, 0.2f, 0.2f);

    // Componentes
    private CharacterController controller;

    // Estado de movimento
    private Vector3 velocidade;
    private bool estaNoChao;

    // Estado de stamina
    private float staminaAtual;
    private float timerRecuperacao;
    private bool podeRecuperar;
    private bool staminaEsgotada;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        staminaAtual = staminaMaxima;
        podeRecuperar = true;

        if (barraStamina != null)
        {
            barraStamina.maxValue = staminaMaxima;
            barraStamina.value = staminaAtual;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TratarMovimento();
        TratarStamina();
        AtualizarUI();
    }

    private void TratarMovimento()
    {
        // Detecta se está no chão e reseta velocidade vertical
        estaNoChao = controller.isGrounded;
        if (estaNoChao && velocidade.y < 0)
            velocidade.y = -2f;

        // Lê o input WASD / setas
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direcao = transform.right * horizontal + transform.forward * vertical;

        // Define se pode correr: Shift pressionado, stamina disponível e há movimento
        bool tentandoCorrer = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool estaMovendo = direcao.sqrMagnitude > 0.01f;
        bool correndo = tentandoCorrer && estaMovendo && !staminaEsgotada;

        float velocidadeAtual = correndo ? velocidadeCorrer : velocidadeAndar;

        // Move o personagem
        controller.Move(direcao * velocidadeAtual * Time.deltaTime);

        // Aplica gravidade
        velocidade.y += gravidade * Time.deltaTime;
        controller.Move(velocidade * Time.deltaTime);
    }

    private void TratarStamina()
    {
        bool tentandoCorrer = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool estaMovendo = new Vector2(horizontal, vertical).sqrMagnitude > 0.01f;
        bool correndo = tentandoCorrer && estaMovendo && !staminaEsgotada;

        if (correndo)
        {
            // Gasta stamina enquanto corre
            staminaAtual -= staminaGastoPorSegundo * Time.deltaTime;
            staminaAtual = Mathf.Clamp(staminaAtual, 0f, staminaMaxima);

            // Reseta o timer de recuperação enquanto corre
            timerRecuperacao = 0f;
            podeRecuperar = false;

            // Marca como esgotada se chegar a zero
            if (staminaAtual <= 0f)
                staminaEsgotada = true;
        }
        else
        {
            // Aguarda o delay antes de começar a recuperar
            if (!podeRecuperar)
            {
                timerRecuperacao += Time.deltaTime;
                if (timerRecuperacao >= delayParaRecuperar)
                    podeRecuperar = true;
            }

            // Recupera stamina após o delay
            if (podeRecuperar && staminaAtual < staminaMaxima)
            {
                staminaAtual += staminaRecuperacaoPorSegundo * Time.deltaTime;
                staminaAtual = Mathf.Clamp(staminaAtual, 0f, staminaMaxima);
            }

            // Libera o estado de esgotamento ao recuperar pelo menos 30% da stamina
            if (staminaEsgotada && staminaAtual >= staminaMaxima * 0.3f)
                staminaEsgotada = false;
        }
    }

    private void AtualizarUI()
    {
        if (barraStamina == null) return;

        barraStamina.value = staminaAtual;

        // Muda a cor da barra conforme o nível de stamina
        if (preenchimentoStamina != null)
        {
            float proporcao = staminaAtual / staminaMaxima;
            preenchimentoStamina.color = Color.Lerp(corStaminaBaixa, corStaminaCheia, proporcao);
        }
    }

    // --- Propriedades públicas para leitura externa (HUD, animações, etc.) ---

    /// <summary>Stamina atual do jogador (0 a StaminaMaxima).</summary>
    public float StaminaAtual => staminaAtual;

    /// <summary>Stamina máxima configurada.</summary>
    public float StaminaMaxima => staminaMaxima;

    /// <summary>Retorna true se o personagem está correndo neste frame.</summary>
    public bool EstaCorrend()
    {
        bool tentando = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool movendo = new Vector2(h, v).sqrMagnitude > 0.01f;
        return tentando && movendo && !staminaEsgotada;
    }
}
