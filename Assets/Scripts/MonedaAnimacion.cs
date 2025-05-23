using UnityEngine;

public class MonedaAnimacion : MonoBehaviour
{
    // Parámetros de rotación
    public float velocidadRotacion = 50f;

    // Parámetros de flotación
    public float alturaFlotacion = 0.3f;
    public float velocidadFlotacion = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Girar sobre el eje Y (vertical)
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.World);

        // Flotar (subir y bajar)
        float nuevaY = Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
        transform.position = new Vector3(posicionInicial.x, posicionInicial.y + nuevaY, posicionInicial.z);
    }
}
