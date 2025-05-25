using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConfirmacionSalida : MonoBehaviour
{
    public AudioSource sonidoClick;
    private string escenaAnterior;

    private void Start()
    {
        // Obtén la escena anterior guardada, por ejemplo "Inicio" si no existe
        escenaAnterior = PlayerPrefs.GetString("escenaAnterior", "Inicio"); 
    }

    public void ConfirmarSalida()
    {
        if (sonidoClick != null)
        {
            sonidoClick.Play();
            StartCoroutine(SalirDespuesDelSonido());
        }
        else
        {
            Application.Quit();
        }
    }

    public void CancelarSalida()
    {
        if (sonidoClick != null)
        {
            sonidoClick.Play();
            StartCoroutine(VolverDespuesDelSonido());
        }
        else
        {
            if (!string.IsNullOrEmpty(escenaAnterior))
                SceneManager.LoadScene(escenaAnterior);
            else
                SceneManager.LoadScene("Inicio"); // Escena por defecto si no hay valor guardado
        }
    }

    private IEnumerator SalirDespuesDelSonido()
    {
        yield return new WaitWhile(() => sonidoClick.isPlaying);
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    private IEnumerator VolverDespuesDelSonido()
    {
        yield return new WaitWhile(() => sonidoClick.isPlaying);

        if (!string.IsNullOrEmpty(escenaAnterior))
        {
            SceneManager.LoadScene(escenaAnterior);
        }
        else
        {
            SceneManager.LoadScene("Inicio"); // Escena por defecto
        }
    }
}
