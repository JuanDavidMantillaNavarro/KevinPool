using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public string nombreEscenaConfirmacion = "ConfirmarSalida"; // Cambia si es necesario
    public AudioSource sonidoClick;

    private string escenaAnterior;

    private void Start()
    {
        escenaAnterior = PlayerPrefs.GetString("escenaAnterior", "Inicio");
    }

    public void VolverAtras()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(CargarEscenaDespuesDelSonido(escenaAnterior));
    }

    public void IrADobleConfirmacion()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(CargarEscenaDespuesDelSonido(nombreEscenaConfirmacion));
    }

    private IEnumerator CargarEscenaDespuesDelSonido(string nombreEscena)
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        SceneManager.LoadScene(nombreEscena);
    }
}