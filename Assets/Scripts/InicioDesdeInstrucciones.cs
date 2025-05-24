using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InicioDesdeInstrucciones : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float duracionFade = 1f;
    public string nombreEscenaJuego = "SampleScene";
    public string nombreEscenaInicio = "Inicio"; // ← Nombre exacto de tu menú principal
    public AudioSource sonidoClick;

    private bool yaEjecutado = false;

    public void IniciarJuego()
    {
        if (yaEjecutado) return;
        yaEjecutado = true;

        if (sonidoClick != null && !sonidoClick.isPlaying)
            sonidoClick.Play();

        StartCoroutine(FadeYIrAEscena(nombreEscenaJuego));
    }

    public void VolverAlInicio()
    {
        if (yaEjecutado) return;
        yaEjecutado = true;

        if (sonidoClick != null && !sonidoClick.isPlaying)
            sonidoClick.Play();

        StartCoroutine(FadeYIrAEscena(nombreEscenaInicio));
    }

    private IEnumerator FadeYIrAEscena(string nombreEscena)
    {
        if (sonidoClick != null)
            yield return new WaitForSeconds(sonidoClick.clip.length);

        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = false;

        LeanTween.alphaCanvas(fadeCanvasGroup, 1f, duracionFade).setOnComplete(() =>
        {
            SceneManager.LoadScene(nombreEscena);
        });
    }
}
