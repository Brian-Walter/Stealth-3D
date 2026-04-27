using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryLoader : MonoBehaviour
{
    public static void CarregarVitoria()
    {
        PlayerPrefs.SetInt("Vitoria", 1);
        SceneManager.LoadScene("Menu_Scene");
    }
}