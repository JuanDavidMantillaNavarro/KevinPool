using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Panel Game Over")]
    public GameObject panelGameOver;   // Panel con imagen y botones de Game Over

    [Header("Opcional: Fade para transiciones")]
    public CanvasGroup fadeCanvasGroup;
    public float duracionFade = 1f;

    private void Start()
    {
        panelGameOver.SetActive(false);  // Empieza oculto
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(true);
        }
    }

    public void MostrarGameOver()
    {
        panelGameOver.SetActive(true);
        Time.timeScale = 0f;  // Pausa el juego
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;  // Reanuda el juego
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            fadeCanvasGroup.interactable = false;

            LeanTween.alphaCanvas(fadeCanvasGroup, 1f, duracionFade).setOnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            fadeCanvasGroup.interactable = false;

            LeanTween.alphaCanvas(fadeCanvasGroup, 1f, duracionFade).setOnComplete(() =>
            {
                SceneManager.LoadScene("Inicio");  // Cambia por el nombre real del menú
            });
        }
        else
        {
            SceneManager.LoadScene("Inicio");
        }
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
