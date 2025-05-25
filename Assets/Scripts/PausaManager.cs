using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaManager : MonoBehaviour
{
    private bool juegoPausado = false;

    public string nombreEscenaSettings = "Settings";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PausarYCambiarEscena()
    {
        if (!juegoPausado)
        {
            juegoPausado = true;

            PlayerPrefs.SetString("escenaAnterior", SceneManager.GetActiveScene().name);

            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscenaSettings);
        }
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
    }
}
