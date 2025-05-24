using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorBolasBlancas : MonoBehaviour
{
    public GameObject bolaBlancaPrefab;
    public BolaScript bolaScript;
    public int maxBolasBlancas = 3;

    private int bolasBlancasExtras = 0; //Contador
    private int ataquesRealizados = 0; //Contador

    private List<GameObject> bolasExtras = new List<GameObject>();
    private List<float> tiemposIA = new List<float>();

    void Update()
    {
        // Tacar con bolas extra segun tiemposIA
        for (int i = 0; i < bolasExtras.Count; i++)
        {
            tiemposIA[i] += Time.deltaTime;
            if (tiemposIA[i] >= 4f) // cada 4 segundos taquean las bolas extra
            {
                TacaBolaExtra(bolasExtras[i]);
                tiemposIA[i] = 0f;
            }
        }
    }

    public void NotificarAtaque()
    {
        ataquesRealizados++;
        if (ataquesRealizados >= 2 && bolasBlancasExtras < maxBolasBlancas - 1)
        {
            AgregarBolaBlancaExtra();
            ataquesRealizados = 0;
        }
    }

    private void AgregarBolaBlancaExtra()
    {
        float alturaY = bolaScript.bolas[0].transform.position.y; //Segun la altura de la original
        Vector3 posicion = new Vector3(Random.Range(-50, 50), alturaY, Random.Range(-100, 100));
        GameObject nuevaBola = Instantiate(bolaBlancaPrefab, posicion, Quaternion.identity);
        nuevaBola.name = "BolaExtra" + bolasBlancasExtras;
        nuevaBola.tag = "Bola";

        // Agregar al sistema de bolas
        List<GameObject> nuevasBolas = new List<GameObject>(bolaScript.bolas);
        nuevasBolas.Add(nuevaBola);
        bolaScript.bolas = nuevasBolas.ToArray();

        List<Vector3> nuevasVelocidades = new List<Vector3>(bolaScript.GetVelocidades());
        nuevasVelocidades.Add(Vector3.zero); // comenzamos sin velocidad
        bolaScript.SetVelocidades(nuevasVelocidades.ToArray());

        bolasExtras.Add(nuevaBola);
        tiemposIA.Add(0f); // para controlar el tiempo de taqueo

        bolasBlancasExtras++;
    }

    private void TacaBolaExtra(GameObject bola)
    {
        int index = System.Array.IndexOf(bolaScript.bolas, bola);
        if (index >= 0)
        {
            Vector3 direccion = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            Vector3[] vels = bolaScript.GetVelocidades();
            vels[index] = direccion * bolaScript.GetVelocidadTiro();
            bolaScript.SetVelocidades(vels);

            // Audio (para colocar alguno)
            // bolaScript.GetComponent<AudioSource>().Play();

            // para que no vuele y se quede en el paño correctamente
            bola.transform.position = new Vector3(bola.transform.position.x, bolaScript.bolas[0].transform.position.y, bola.transform.position.z);
        }
    }
}
