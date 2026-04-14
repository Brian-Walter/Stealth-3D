using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelCreditos;
    [SerializeField] private GameObject painelGameOver;

    // Funcao chamada quando o jogador clica em "Iniciar Jogo"
    // Aqui voce pode colocar o codigo para carregar a cena do jogo
    public void IniciarJogo()
    {
        painelMenuInicial.SetActive(false);
        SceneManager.LoadScene("CenaPrincipal");
    }

    // Abre o painel de opcoes e esconde o menu inicial
    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);

    }

    // Fecha o painel de opcoes e volta para o menu inicial
    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }
    // Abre o painel de creditos e esconde o menu inicial
    public void AbrirCreditos()
    {
        painelCreditos.SetActive(true);
        painelMenuInicial.SetActive(false);
    }

    // Fecha o painel de cresditos e volta para o menu inicial
    public void SairCreditos()
    {
        painelCreditos.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    // Fecha o painel de Game Over e volta para o menu inicial
    public void SairGameOver()
    {
        painelGameOver.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

}
