using UnityEngine;

public class Moneda : MonoBehaviour
{
    public MonedaManager manager;

    // Radios reales
    private float radioJugador = 0.36f;
    private float radioMoneda = 3.0f;

    private GameObject jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.transform.position);

        if (distancia < radioJugador + radioMoneda)
        {
            manager.ReemplazarMoneda(gameObject);
            Destroy(gameObject);
        }
    }
}
