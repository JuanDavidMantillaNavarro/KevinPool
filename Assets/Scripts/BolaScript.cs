using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;


public class BolaScript : MonoBehaviour
{
    public float masa = 0.170f, e = 0.9f, epared = 1f, radio = ((5.7f) / 2f), friccionFactor = 0.995f, vIni = 135f;
    private float BolaQuieta = 0f;
    private bool espera = false;
    
    List<GameObject> ordbolas = new List<GameObject>();  // Arreglo ordenado de las bolas
    public GameObject[] bolas;  // Arreglo para todas las bolas
    private Vector3[] velocidades;  // Para almacenar las velocidades de cada bola

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar las bolas y sus velocidades
        bolas = new GameObject[16]; 
        velocidades = new Vector3[16]; //velocidades de cada bola

        GameObject[] allBolas = GameObject.FindGameObjectsWithTag("Bola");

        foreach (GameObject bola in allBolas)
        {
            if (bola.name == "Bola")
            {
                ordbolas.Insert(0, bola);
                break;
            }
        }
         var ordenadas = allBolas
            .Where(b => b.name != "Bola" && Regex.IsMatch(b.name, @"^Bola\d+$"))
            .OrderBy(b =>
            {
                string numberPart = Regex.Match(b.name, @"\d+").Value;
                return int.Parse(numberPart);
            });

        ordbolas.AddRange(ordenadas);

        bolas = ordbolas.ToArray();

        // bola blanca se mueve inicialmente
        velocidades[0] = new Vector3(0.1f, 0, vIni); 
        for (int i = 1; i < bolas.Length; i++)
        {
            velocidades[i] = Vector3.zero; // Las bolas 2 a 16 estan inicialmente quietas
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar las posiciones de todas las bolas
        for (int i = 0; i < bolas.Length; i++)
        {
            bolas[i].transform.position += velocidades[i] * Time.deltaTime;
        }

        // Verificar las colisiones con las paredes
        for (int i = 0; i < bolas.Length; i++)
        {
            VerificarColisionParedes(i);
        }

        // Verificar las colisiones entre todas las bolas
        for (int i = 0; i < bolas.Length; i++)
        {
            for (int j = i + 1; j < bolas.Length; j++)
            {
                VerificarColisionEntreBolas(i, j);
            }
        }

        // Aplicar la fricción a todas las bolas
        for (int i = 0; i < bolas.Length; i++)
        {
            Friccion(i);
        }

        //Cuando la bola blanca se detiene  espera un tiempo y vuelve a moverse a una direccion aleatoria}
        if (velocidades[0].magnitude < 0.8f)
        {
        BolaQuieta += Time.deltaTime;

            if (BolaQuieta >= 0.01f && !espera)
            {
                espera = true;
                StartCoroutine(TacarBola());
            }
        }
        else
        {
            BolaQuieta = 0f;
            espera = false;
        }
    }

    private IEnumerator TacarBola()
    {
        // Espera un tiempo minimo
        yield return new WaitForSeconds(0.05f);

        // Escoge un índice aleatorio de una bola distinta a la blanca
        int bolaAtacar = UnityEngine.Random.Range(1, bolas.Length);

        // tacar bola random en el plano XZ
        Vector3 direccion = (bolas[bolaAtacar].transform.position - bolas[0].transform.position).normalized;

        float vTaqueo = 400f;
        velocidades[0] = direccion * vTaqueo;

        BolaQuieta = 0f;
        espera = false;

        Debug.Log("La Bola Blanca va a Tacar la Bola "+ bolaAtacar);
    }

    private void VerificarColisionParedes(int index)
    {
        Vector3 pos = bolas[index].transform.position;

        // Colisión con las paredes
        // Rebotar en el eje Z (paredes de largo)
        if (pos.z < -137 + radio)
        {
        velocidades[index].z = -epared * velocidades[index].z;
        pos.z = -137 + radio; 
        }
        else if (pos.z > 137 - radio)
        {
        velocidades[index].z = -epared * velocidades[index].z;
        pos.z = 137 - radio; 
        }

        // Rebotar en el eje X (paredes de ancho)
        if (pos.x < -73 + radio)
        {
        velocidades[index].x = -epared * velocidades[index].x;
        pos.x = -73 + radio; 
        }
        else if (pos.x > 73 - radio)
        {
        velocidades[index].x = -epared * velocidades[index].x;
        pos.x = 73 - radio; 
        }

        bolas[index].transform.position = pos;
    }

    private void VerificarColisionEntreBolas(int index1, int index2)
    {
        // Trabajamos con posiciones en 3D (pero solo la distancia en X y Z)
        Vector3 pos1 = bolas[index1].transform.position;
        Vector3 pos2 = bolas[index2].transform.position;

        // Calcular la distancia en 3D entre las dos bolas
        float distanciaEntreBolas = Vector3.Distance(pos1, pos2);

        if (distanciaEntreBolas < 2 * radio) // Si hay colisión cuando la distancia es menor que el doble del radio
        {
            Vector3 normal = (pos2 - pos1).normalized; // Se saca la normal

            // Velocidades normales y tangenciales
            float v1n = Vector3.Dot(velocidades[index1], normal); // Velocidad en la normal
            float v1t = Vector3.Dot(velocidades[index1], Vector3.Cross(normal, Vector3.up)); // Componente tangencial
            float v2n = Vector3.Dot(velocidades[index2], normal); // Velocidad en la normal
            float v2t = Vector3.Dot(velocidades[index2], Vector3.Cross(normal, Vector3.up)); // Componente tangencial

            // Si las velocidades normales son muy pequeñas, podemos evitar la colisión
            if (Mathf.Abs(v1n) < 0.001f && Mathf.Abs(v2n) < 0.001f)
                return;

            // Ecuaciones de colisión elástica
            float aux = v1n + v2n;
            float aux2 = e * (v1n - v2n);
            float vf1n = 0, vf2n = 0;
            vf1n = aux - vf2n;
            vf2n = aux2 + vf1n;

            // Limitar la velocidad de la bola a un rango más razonable
            float maxVel = 400f;  // Puedes ajustar este valor según lo que necesites
            vf1n = Mathf.Clamp(vf1n, -maxVel, maxVel);
            vf2n = Mathf.Clamp(vf2n, -maxVel, maxVel);

            // Ajustar las velocidades para reducir el impacto
            float amortiguacion = 0.85f; // Factor de amortiguación para moderar el cambio de velocidad
            vf1n *= amortiguacion;
            vf2n *= amortiguacion;

            // Nuevas velocidades (para X y Z) por producto punto
            velocidades[index1].x = vf1n * normal.x + v1t * Vector3.Cross(normal, Vector3.up).x;
            velocidades[index1].z = vf1n * normal.z + v1t * Vector3.Cross(normal, Vector3.up).z;
            velocidades[index2].x = vf2n * normal.x + v2t * Vector3.Cross(normal, Vector3.up).x;
            velocidades[index2].z = vf2n * normal.z + v2t * Vector3.Cross(normal, Vector3.up).z;
            
            float solapamiento = (2 * radio - distanciaEntreBolas) / 2f;
            Vector3 separacion = normal * solapamiento;
            bolas[index1].transform.position -= separacion;
            bolas[index2].transform.position += separacion;

        }
    }

    // aplicar la fricción a las bolas por la mesa
    private void Friccion(int index)
    {
        // Si la magnitud de la velocidad es suficientemente baja, podemos considerar que la bola está detenida.
        if (velocidades[index].magnitude < 0.005f)
        {
        velocidades[index] = Vector3.zero; // Detener la bola
        }
        else
        {
            // Solo aplicar fricción si la bola se esta moviendo
            if (velocidades[index].magnitude > 0.001f)
            {
            velocidades[index] *= friccionFactor; 
            }
            // Si la velocidad es demasiado baja, ponerla a cero para evitar que se mueva para el otro lado
            if (Mathf.Abs(velocidades[index].x) < 0.001f) velocidades[index].x = 0;
            if (Mathf.Abs(velocidades[index].z) < 0.001f) velocidades[index].z = 0;
        }
        
    }
}
