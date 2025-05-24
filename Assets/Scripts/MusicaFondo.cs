using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    public static MusicaFondo Instance;

    public AudioSource audioSource;
    public float volumenObjetivo = 1f;
    public float duracionFade = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource.volume = 0f;
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void CambiarMusica(AudioClip nuevaMusica)
    {
        StartCoroutine(FadeOutAndChange(nuevaMusica));
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volumenObjetivo, tiempo / duracionFade);
            yield return null;
        }
    }

    private System.Collections.IEnumerator FadeOutAndChange(AudioClip nuevaMusica)
    {
        float tiempo = 0f;
        float volumenInicial = audioSource.volume;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volumenInicial, 0f, tiempo / duracionFade);
            yield return null;
        }

        audioSource.clip = nuevaMusica;
        audioSource.Play();

        // Fade In nueva música
        tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volumenObjetivo, tiempo / duracionFade);
            yield return null;
        }
    }
}
