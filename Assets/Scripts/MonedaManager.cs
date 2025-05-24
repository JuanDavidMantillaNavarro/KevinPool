using UnityEngine;
using System.Collections.Generic;

public class MonedaManager : MonoBehaviour
{
    public GameObject monedaPrefab;
    public Vector3 mesaSize = new Vector3(146, 1, 274); // Tama�o del pa�o
    public float alturaMoneda = 3.58f; // altura del paño
    public float separacionMinima = 8f;
    public int cantidadMonedas = 15;

    private List<GameObject> monedas = new List<GameObject>();

    void Start()
    {
        GenerarMonedasIniciales();
    }

    void GenerarMonedasIniciales()
    {
        int intentos = 0;

        while (monedas.Count < cantidadMonedas && intentos < 10000)
        {
            intentos++;
            Vector3 posicion = ObtenerPosicionAleatoria();

            if (EsPosicionValida(posicion))
            {
                GameObject nueva = Instantiate(monedaPrefab, posicion, Quaternion.Euler(90, 0, 0)); // Rotada como moneda vertical
                nueva.GetComponent<Moneda>().manager = this;
                monedas.Add(nueva);
            }
        }
    }

    public void ReemplazarMoneda(GameObject monedaEliminada)
    {
        monedas.Remove(monedaEliminada);

        int intentos = 0;
        Vector3 posicion;

        do
        {
            posicion = ObtenerPosicionAleatoria();
            intentos++;
        } while (!EsPosicionValida(posicion) && intentos < 100);

        GameObject nueva = Instantiate(monedaPrefab, posicion, Quaternion.Euler(90, 0, 0));
        nueva.GetComponent<Moneda>().manager = this;
        monedas.Add(nueva);
    }

    private Vector3 ObtenerPosicionAleatoria()
    {
        float x = Random.Range(-mesaSize.x / 2 + separacionMinima, mesaSize.x / 2 - separacionMinima);
        float z = Random.Range(-mesaSize.z / 2 + separacionMinima, mesaSize.z / 2 - separacionMinima);
        return new Vector3(x, alturaMoneda, z);
    }

    private bool EsPosicionValida(Vector3 nuevaPos)
    {
        foreach (GameObject m in monedas)
        {
            if (Vector3.Distance(m.transform.position, nuevaPos) < separacionMinima)
                return false;
        }
        return true;
    }
}
