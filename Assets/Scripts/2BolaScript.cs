using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BolaScript2 : MonoBehaviour
{
    public float masa = 0.170f, zac1, xac1, vac1, zac2, xac2, vac2, e = 0.5f, epared = 0.2f, radio = ((5.7f)/2f), xaux = 0f, v1x = 0.03f, v1z = 135f, v1,v2, v1n, v1t, v2n, v2t, ang,ang2, v2x = 0.03f, v2z = 0.03f, distancia ,vf1n, vf2n;
    public GameObject Bola1, Bola2;

    // Start is called before the first frame update
    void Start()
    {
        zac1 = this.transform.position.z;
        xac1 = this.transform.position.x;
        vac1 = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //ACTUALIZAR POSICION
        // Actualiza las posiciones, pero mantén Y fijo a 3.3
        Bola1.transform.position = new Vector3(Bola1.transform.position.x + v1x * Time.deltaTime, 3.3f, Bola1.transform.position.z + v1z * Time.deltaTime);
        Bola2.transform.position = new Vector3(Bola2.transform.position.x + v2x * Time.deltaTime, 3.3f, Bola2.transform.position.z + v2z * Time.deltaTime);


        //Pos en x act
        xac1 = Bola1.transform.position.x;
        xac2 = Bola2.transform.position.x;
        //Pos en z act
        zac1 = Bola1.transform.position.z;
        zac2 = Bola2.transform.position.z;

        VerificarColisionParedes();
        VerificarColisionEntreBolas();

        Friccion();
        

    }
    private void VerificarColisionParedes()
    {
        //Distancia con pared
        //Paredes en -8.5 y 8.5

        if (zac1 < -137 + radio || zac1 > 137 - radio)
        {
            v1z = -epared * v1z;
            //Debug.Log("choco verticalmente");
        }
        else if (xac1 < -73 + radio || xac1 > 73 - radio)
        {
            v1x = -epared * v1x;
            //Debug.Log("choco horizontalmente");
        }

        if (zac2 < -137 + radio || zac2 > 137 - radio)
        {
            v2z = -epared * v2z;
            //Debug.Log("choco verticalmente la bola 2");
        }
        else if (xac2 < -73 + radio || xac2 > 73 - radio)
        {
            v2x = -epared * v2x;
            //Debug.Log("choco horizontalmente la bola 2");
        }
    }

    void VerificarColisionEntreBolas()
    {
        // Trabajamos con posiciones en 3D (pero solo la distancia en X y Z)
        Vector3 pos1 = Bola1.transform.position;
        Vector3 pos2 = Bola2.transform.position;

        // Calcular la distancia en 3D entre las dos bolas
        float distanciaEntreBolas = Vector3.Distance(pos1, pos2);

        if (distanciaEntreBolas < 2 * radio) // Si hay colision cuando la distancia es menor que el doble del radio
        {
            
            Vector3 normal = (pos2 - pos1).normalized;// Se saca la normal

            // velocidades normales y tangenciales
            float v1n = Vector3.Dot(new Vector3(v1x, 0, v1z), normal); // velocidad en la normal
            float v1t = Vector3.Dot(new Vector3(v1x, 0, v1z), Vector3.Cross(normal, Vector3.up)); // Componente tangencial
            float v2n = Vector3.Dot(new Vector3(v2x, 0, v2z), normal); // velocidad en la normal
            float v2t = Vector3.Dot(new Vector3(v2x, 0, v2z), Vector3.Cross(normal, Vector3.up)); // Componente tangencial

            // ecuaciones de colision elastica
            // Usando las ecuaciones: v1n + v2n = vf1n + vf2n; y vf2n - vf1n = e(v1n - v2n);
            float aux = v1n + v2n;
            float aux2 = e * (v1n - v2n);
            vf1n = aux - vf2n;
            vf2n = aux2 + vf1n;

            // Nuevas velocidades (para X y Z) por producto punto
            v1x = vf1n * normal.x + v1t * Vector3.Cross(normal, Vector3.up).x;
            v1z = vf1n * normal.z + v1t * Vector3.Cross(normal, Vector3.up).z;
            v2x = vf2n * normal.x + v2t * Vector3.Cross(normal, Vector3.up).x;
            v2z = vf2n * normal.z + v2t * Vector3.Cross(normal, Vector3.up).z;

            
        }
    }


     // Metodo para aplicar la friccion a las bolas por la mesa
    private void Friccion()
    {
        float friccion = 0.993f; // Factor de friccion

        v1x *= friccion;
        v1z *= friccion;
        v2x *= friccion;
        v2z *= friccion;

        // Si la velocidad es demasiado baja, ponerla a cero para evitar que se mueva para el otro lado
        if (Mathf.Abs(v1x) < 0.001f) v1x = 0;
        if (Mathf.Abs(v1z) < 0.001f) v1z = 0;
        if (Mathf.Abs(v2x) < 0.001f) v2x = 0;
        if (Mathf.Abs(v2z) < 0.001f) v2z = 0;
    }
}


