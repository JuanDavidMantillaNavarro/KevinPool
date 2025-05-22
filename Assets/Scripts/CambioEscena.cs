using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonConTransicion : MonoBehaviour
{
    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolver = 1.5f;
    public string escenaDestino = "SampleScene"; // Asegúrate que esté en Build Settings

    public void CambiarEscenaConFade()
    {
        disolverCanvasGroup.alpha = 0f;
        disolverCanvasGroup.gameObject.SetActive(true);

        LeanTween.alphaCanvas(disolverCanvasGroup, 1f, tiempoDisolver).setOnComplete(() =>
        {
            SceneManager.LoadScene(escenaDestino);
        });
    }
}
