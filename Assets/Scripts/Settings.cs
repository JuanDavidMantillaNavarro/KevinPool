using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("Fade y Transición")]
    public CanvasGroup fadeCanvasGroup;
    public float duracionFade = 1f;
    public string nombreEscenaConfirmacion = "ConfirmacionSalida";
    public AudioSource sonidoClick;

    [Header("Opciones de Audio y Calidad")]
    public AudioMixer audioMixer;
    public TMP_Dropdown dropdownCalidad;

    private string escenaAnterior;

    private void Start()
    {
        escenaAnterior = PlayerPrefs.GetString("escenaAnterior", "Inicio");

        // Fade de entrada
        fadeCanvasGroup.alpha = 1f;
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = false;
        LeanTween.alphaCanvas(fadeCanvasGroup, 0f, duracionFade);

        // Establecer calidad gráfica
        int calidadGuardada = PlayerPrefs.GetInt("CalidadGrafica", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(calidadGuardada);

        if (dropdownCalidad != null)
        {
            dropdownCalidad.ClearOptions();
            dropdownCalidad.AddOptions(new List<string>(QualitySettings.names));
            dropdownCalidad.value = calidadGuardada;
            dropdownCalidad.RefreshShownValue();
        }
    }

    public void VolverAtras()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(FadeYIrAEscena(escenaAnterior));
    }

    public void IrADobleConfirmacion()
    {
        if (sonidoClick != null)
            sonidoClick.Play();

        StartCoroutine(FadeYIrAEscena(nombreEscenaConfirmacion));
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

    public void CambiarVolumenMusica(float volumen)
    {
        float volumenEnDb = Mathf.Log10(Mathf.Max(volumen, 0.0001f)) * 20f;
        audioMixer.SetFloat("VolumenMusica", volumenEnDb);
    }

    public void CambiarCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("CalidadGrafica", index);
    }
}
