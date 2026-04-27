using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLoader : MonoBehaviour
{
    public static void CarregarGameOver(MonoBehaviour caller, float delay = 1.5f)
    {
        caller.StartCoroutine(CarregarGameOverComDelay(delay));
    }

    private static IEnumerator CarregarGameOverComDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayerPrefs.SetInt("GameOver", 1);
        SceneManager.LoadScene("Menu_Scene");
    }
}