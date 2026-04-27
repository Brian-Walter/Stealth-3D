using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Painéis")]
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelCreditos;
    [SerializeField] private GameObject painelGameOver;
    [SerializeField] private GameObject painelVitoria;

    private void Start()
    {
        // Cursor visível no menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Estados vindos de outras cenas
        bool veioDoGameOver = PlayerPrefs.GetInt("GameOver", 0) == 1;
        bool veioDaVitoria = PlayerPrefs.GetInt("Vitoria", 0) == 1;

        // Controle de telas
        painelMenuInicial.SetActive(!veioDoGameOver && !veioDaVitoria);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
        painelGameOver.SetActive(veioDoGameOver);
        painelVitoria.SetActive(veioDaVitoria);

        // Reset flags
        PlayerPrefs.SetInt("GameOver", 0);
        PlayerPrefs.SetInt("Vitoria", 0);
    }

    // ===== JOGO =====

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Main_Scene");
    }

    public void TentarNovamente()
    {
        SceneManager.LoadScene("Main_Scene");
    }

    // ===== GAME OVER =====

    public void SairGameOver()
    {
        painelGameOver.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    // ===== VITÓRIA =====

    public void SairVitoria()
    {
        painelVitoria.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void JogarNovamente()
    {
        SceneManager.LoadScene("Main_Scene");
    }

    // ===== OPÇÕES =====

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    // ===== CRÉDITOS =====

    public void AbrirCreditos()
    {
        painelCreditos.SetActive(true);
        painelMenuInicial.SetActive(false);
    }

    public void SairCreditos()
    {
        painelCreditos.SetActive(false);
        painelMenuInicial.SetActive(true);
    }
}