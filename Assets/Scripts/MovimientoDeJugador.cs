using UnityEngine;

public class PlayerMovementManual : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    public LayerMask groundLayer;

    private CapsuleCollider capsuleCollider;
    private Vector3 velocity;
    private bool isGrounded;

    // Límites del área de juego (ajusta según tu escenario)
    public float minX = -73f;
    public float maxX = 73f;
    public float minZ = -137f;
    public float maxZ = 137f;

    public float radio = 0.5f;  // Radio para que el jugador no traspase la pared (ajusta según collider)
    public float epared = 0.8f; // Coeficiente de rebote, entre 0 y 1

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // Detección de suelo (igual que antes)
        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius + 0.05f);

        isGrounded = Physics.OverlapCapsule(point1, point2, capsuleCollider.radius * 2.5f, groundLayer).Length > 0;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0f;

        // Movimiento WASD (normalizado)
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move += -transform.forward;
        if (Input.GetKey(KeyCode.A)) move += -transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        move = move.normalized * moveSpeed * Time.deltaTime;

        // Aplico movimiento horizontal
        Vector3 nuevaPos = transform.position + move;

        // Aquí simulo rebote / restricción contra paredes

        // Rebote o ajuste eje Z
        if (nuevaPos.z < -137 + radio)
        {
            // Rebote: invierto velocidad horizontal z (como ejemplo)
            velocity.z = -epared * velocity.z; // Si usas velocity.z
            nuevaPos.z = -137 + radio;
        }
        else if (nuevaPos.z > 137 - radio)
        {
            velocity.z = -epared * velocity.z;
            nuevaPos.z = 137 - radio;
        }

        // Rebote o ajuste eje X
        if (nuevaPos.x < -73 + radio)
        {
            velocity.x = -epared * velocity.x;
            nuevaPos.x = -73 + radio;
        }
        else if (nuevaPos.x > 73 - radio)
        {
            velocity.x = -epared * velocity.x;
            nuevaPos.x = 73 - radio;
        }

        transform.position = nuevaPos;

        // Salto
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = jumpForce;

        // Gravedad
        if (!isGrounded)
            velocity.y -= gravity * Time.deltaTime;
        else if (velocity.y < 0)
            velocity.y = -2f;

        // Movimiento vertical
        transform.position += new Vector3(0, velocity.y * Time.deltaTime, 0);
    }
}
