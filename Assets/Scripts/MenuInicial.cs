using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{
    public AudioSource sonidoClick;

    [Header("Nombres exactos de las escenas")]
    public string nombreEscenaInstrucciones = "instrucciones";
    public string nombreEscenaSettings = "Settings";
    public string nombreEscenaConfirmacion = "ConfirmacionSalida";

    public void Jugar()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(nombreEscenaInstrucciones));
    }

    public void Settings()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(nombreEscenaSettings));
    }

    public void Salir()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(nombreEscenaConfirmacion));
    }

    private IEnumerator TransicionConSonido(string nombreEscena)
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        SceneManager.LoadScene(nombreEscena);
    }
}
