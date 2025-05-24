using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;                           // El jugador (asigna en Inspector)
    public Vector3 offset = new Vector3(0f, 5f, -7f);   // Altura y distancia de la cámara
    public float followSpeed = 5f;                      // Suavidad del seguimiento
    public float rotateSpeed = 100f;                    // Velocidad de rotación horizontal

    private float yaw = 0f; // Ángulo de rotación en el eje Y

    void LateUpdate()
    {
        // 1. Rotar con el mouse en horizontal
        yaw += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;

        // 2. Calcular la rotación y posición deseada
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        // 3. Mover la cámara suavemente a esa posición
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 4. Hacer que mire al centro del jugador (puedes ajustar el up para apuntar más bajo o alto)
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}