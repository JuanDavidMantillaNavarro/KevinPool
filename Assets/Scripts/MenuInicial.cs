using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{
    public AudioSource sonidoClick;

    public int indiceEscenaInstrucciones = 1;   // ✅ Nueva: escena de instrucciones
    public int indiceEscenaSettings = 2;
    public int indiceEscenaConfirmacion = 3;

    public void Jugar()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        // ✅ Ahora va a la escena de instrucciones
        StartCoroutine(TransicionConSonido(indiceEscenaInstrucciones));
    }

    public void Settings()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(indiceEscenaSettings));
    }

    public void Salir()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(TransicionConSonido(indiceEscenaConfirmacion));
    }

    private IEnumerator TransicionConSonido(int indiceEscena)
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        TransicionEscenasUI1.Instance.DisolverSalida(indiceEscena);
    }
}
