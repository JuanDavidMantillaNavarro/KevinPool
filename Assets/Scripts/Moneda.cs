using UnityEngine;

public class Moneda : MonoBehaviour
{
    public MonedaManager manager;

    private float radioJugador = 0.36f;
    private float radioMoneda = 3.0f;

    private GameObject jugador;
    private bool recogida = false;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
    }

    void Update()
    {
        if (jugador == null || recogida) return;

        float distancia = Vector3.Distance(transform.position, jugador.transform.position);

        if (distancia < radioJugador + radioMoneda)
        {
            recogida = true;
            manager.RegistrarRecoleccion(this.gameObject);
        }
    }
}
