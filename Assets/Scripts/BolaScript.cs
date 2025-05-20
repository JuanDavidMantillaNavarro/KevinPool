using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BolaScript : MonoBehaviour
{
    public float masa = 0.170f, yac1, xac1, vac1, yac2, xac2, vac2, e = 0.8f, radio = 0.5f, xaux = 0f, v1x = -30f, v1y = -13f, v1,v2, v1n, v1t, v2n, v2t, ang,ang2, v2x = 2f, v2y = 5f, distancia ,vf1n, vf2n;
    public GameObject Bola1, Bola2;

    // Start is called before the first frame update
    void Start()
    {
        yac1 = this.transform.position.y;
        xac1 = this.transform.position.x;
        vac1 = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //ACTUALIZAR POSICION
        Bola1.transform.position = Bola1.transform.position + new Vector3(v1x * Time.deltaTime, v1y * Time.deltaTime, 0);
        Bola2.transform.position = Bola2.transform.position + new Vector3(v2x * Time.deltaTime, v2y * Time.deltaTime, 0);

        //Pos en x act
        xac1 = Bola1.transform.position.x;
        xac2 = Bola2.transform.position.x;
        //Pos en y act
        yac1 = Bola1.transform.position.y;
        yac2 = Bola2.transform.position.y;

        VerificarColisionParedes();
        VerificarColisionEntreBolas();
        

    }
    private void VerificarColisionParedes()
    {
        //Distancia con pared
        //Paredes en -8.5 y 8.5

        if (yac1 < -4.5 + radio || yac1 > 4.5 - radio)
        {
            v1y = -e * v1y;
            Debug.Log("choco verticalmente");
        }
        else if (xac1 < -8.5 + radio || xac1 > 8.5 - radio)
        {
            v1x = -e * v1x;
            Debug.Log("choco horizontalmente");
        }

        if (yac2 < -4.5 + radio || yac2 > 4.5 - radio)
        {
            v2y = -e * v2y;
            Debug.Log("choco verticalmente la bola 2");
        }
        else if (xac2 < -8.5 + radio || xac2 > 8.5 - radio)
        {
            v2x = -e * v2x;
            Debug.Log("choco horizontalmente la bola 2");
        }
    }

    void VerificarColisionEntreBolas()
    {
        Vector2 pos1 = Bola1.transform.position;
        Vector2 pos2 = Bola2.transform.position;

        //Sacar v1 y v2
        if (v1y < v1x)
        {
            ang = Mathf.Atan(v1x / v1y);
            v1 = v1y / (Mathf.Cos(ang));
        }
        else
        {
            ang = Mathf.Atan(v1y / v1x);
            v1 = v1x / (Mathf.Cos(ang));
        }

        if (v2y < v2x)
        {
            ang2 = Mathf.Atan(v2x / v2y);
            v2 = v2y / (Mathf.Cos(ang2));
        }
        else
        {
            ang2 = Mathf.Atan(v2y / v2x);
            v2 = v2x / (Mathf.Cos(ang2));
        }

        xaux = Mathf.Abs(xac1 - xac2); //Distancia

        if (xaux < 2 * radio) // Si hay colisión
        {
            Vector2 normal = (pos2 - pos1); // se saca la normal

            v1n = v1 * Mathf.Cos(ang);
            v1t = v1 * Mathf.Sin(ang);
            v2n = -v2 * Mathf.Cos(ang2);
            v2t = v2 * Mathf.Sin(ang2);


            //Usando las ecuaciones: v1n + v2n = vf1n + vf2n; y vf2n - vf1n = e(v1n - v2n);
            float aux = v1n + v2n;
            float aux2 = e*(v1n - v2n);
            vf1n = aux - vf2n;
            vf2n = aux2 + vf1n;

            v1x = v1n;
            v1y = v1t;
            v2x = v2n;
            v2y = v2t;

            Vector2 vf1 = new Vector2(vf1n, v1t);
            Vector2 vf2 = new Vector2(vf2n, v2t);


        }
    }
}
