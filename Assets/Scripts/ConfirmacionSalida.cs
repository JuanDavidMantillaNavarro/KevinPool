using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class ConfirmacionSalida : MonoBehaviour
{
    public AudioSource sonidoClick;
    public int indiceEscenaRegresar = 0; // Índice del menú principal

    public void ConfirmarSalida()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(SalirDespuesDelSonido());
    }

    public void CancelarSalida()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(VolverAlMenuDespuesDelSonido());
    }

    private IEnumerator SalirDespuesDelSonido()
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    private IEnumerator VolverAlMenuDespuesDelSonido()
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        SceneManager.LoadScene(indiceEscenaRegresar);
    }
}
