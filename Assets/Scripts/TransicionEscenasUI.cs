using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionEscenasUI : MonoBehaviour
{
    [Header("Disolver")]
    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolverEntrada = 3f;
    public float tiempoAntesDeDisolver = 3f; // Tiempo que dura la portada
    public float tiempoDisolverSalida = 3f;

    [Header("Nombre de la escena a cargar")]
    public string nombreEscenaSiguiente = "Inicio"; // Asegúrate que esté bien escrito

    void Start()
    {
        // Empezamos con el canvas totalmente visible (negro), luego hacemos fade-in
        disolverCanvasGroup.alpha = 1f;
        StartCoroutine(TransicionEntradaYSalida());
    }

    private IEnumerator TransicionEntradaYSalida()
    {
        // === FADE IN ===
        LeanTween.alphaCanvas(disolverCanvasGroup, 0f, tiempoDisolverEntrada);
        yield return new WaitForSeconds(tiempoDisolverEntrada);

        // === Esperar tiempo visible ===
        yield return new WaitForSeconds(tiempoAntesDeDisolver);

        // === FADE OUT ===
        LeanTween.alphaCanvas(disolverCanvasGroup, 1f, tiempoDisolverSalida);
        yield return new WaitForSeconds(tiempoDisolverSalida);

        // === Cargar siguiente escena ===
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}
