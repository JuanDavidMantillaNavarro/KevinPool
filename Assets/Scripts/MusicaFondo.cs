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
            Destroy(gameObject); // Evita duplicados al volver
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
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

    public void FadeOutAndStop()
    {
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float tiempo = 0f;
        float volumenInicial = audioSource.volume;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volumenInicial, 0f, tiempo / duracionFade);
            yield return null;
        }
        audioSource.Stop();
    }
}
