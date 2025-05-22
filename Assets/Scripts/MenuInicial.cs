using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{
    public AudioSource sonidoClick;

    public int indiceEscenaJuego = 1;            // Escena principal del juego
    public int indiceEscenaSettings = 2;         // Escena de configuración
    public int indiceEscenaConfirmacion = 3;     // Escena de confirmación de salida

    public void Jugar()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(indiceEscenaJuego));
    }

    public void Settings()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(indiceEscenaSettings));
    }

    public void ConfirmarSalida()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(indiceEscenaConfirmacion));
    }

    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    private IEnumerator TransicionConSonido(int indiceEscena)
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        TransicionEscenasUI1.Instance.DisolverSalida(indiceEscena);
    }
}
