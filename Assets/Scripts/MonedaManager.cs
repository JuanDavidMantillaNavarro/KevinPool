using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;



public class MonedaManager : MonoBehaviour
{
    public GameObject monedaPrefab;
    public Vector3 mesaSize = new Vector3(146, 1, 274);
    public float alturaMoneda = 3.58f;
    public float separacionMinima = 8f;
    public int cantidadMonedas = 15;

    private List<GameObject> monedas = new List<GameObject>();
    public TextMeshProUGUI contadorTexto;
    private int monedasRecogidas = 0;

    public TextMeshProUGUI timerTexto;
    private float tiempo = 0f;

    void Start()
    {
        GenerarMonedasIniciales();
        ActualizarContadorUI();
    }

    void Update()
    {
        tiempo += Time.deltaTime;
        ActualizarTimerUI();
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
                CrearNuevaMoneda(posicion);
            }
        }
    }

    public void RegistrarRecoleccion(GameObject moneda)
    {
        if (monedas.Contains(moneda))
        {
            monedas.Remove(moneda);
        }

        Destroy(moneda);

        // Actualizar contador
        monedasRecogidas++;
        ActualizarContadorUI();

        // Generar nueva moneda
        int intentos = 0;
        Vector3 nuevaPos;

        do
        {
            nuevaPos = ObtenerPosicionAleatoria();
            intentos++;
        } while (!EsPosicionValida(nuevaPos) && intentos < 100);

        CrearNuevaMoneda(nuevaPos);
    }


    private void CrearNuevaMoneda(Vector3 posicion)
    {
        GameObject nueva = Instantiate(monedaPrefab, posicion, Quaternion.Euler(90, 0, 0));
        nueva.GetComponent<Moneda>().manager = this;
        monedas.Add(nueva);
    }

    private Vector3 ObtenerPosicionAleatoria()
    {
        float x = UnityEngine.Random.Range(-mesaSize.x / 2 + separacionMinima, mesaSize.x / 2 - separacionMinima);
        float z = UnityEngine.Random.Range(-mesaSize.z / 2 + separacionMinima, mesaSize.z / 2 - separacionMinima);
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

    private void ActualizarContadorUI()
    {
        if (contadorTexto != null)
        {
            contadorTexto.text = "Monedas: " + monedasRecogidas;
        }
    }

    private void ActualizarTimerUI()
    {
        if (timerTexto != null)
        {
            TimeSpan tiempoFormateado = TimeSpan.FromSeconds(tiempo);
            timerTexto.text = tiempoFormateado.ToString(@"mm\:ss");
        }  
    }



}
