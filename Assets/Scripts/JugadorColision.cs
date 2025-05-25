using UnityEngine;

public class JugadorColision : MonoBehaviour

{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    public LayerMask groundLayer;

    private CapsuleCollider capsuleCollider;
    private Vector3 velocity;
    private bool isGrounded;

    // Límites del área de juego
    public float minX = -73f;
    public float maxX = 73f;
    public float minZ = -137f;
    public float maxZ = 137f;

    public float radio = 0.5f;  // Radio para evitar traspasar paredes
    public float epared = 0.8f; // Coeficiente de rebote

    [Header("Detección de bolas")]
    public LayerMask capaBolas;          // Capa de las bolas
    public float radioDeteccionBolas = 0.5f;  // Radio para detectar bolas
    public GameOverManager gameOverManager;
    private bool gameOverActivado = false;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // Detección de suelo
        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius + 0.05f);

        isGrounded = Physics.OverlapCapsule(point1, point2, capsuleCollider.radius * 2.5f, groundLayer).Length > 0;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0f;

        // Movimiento horizontal (WASD)
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move += -transform.forward;
        if (Input.GetKey(KeyCode.A)) move += -transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        move = move.normalized * moveSpeed * Time.deltaTime;

        Vector3 nuevaPos = transform.position + move;

        // Restricción y rebote en paredes eje Z
        if (nuevaPos.z < minZ + radio)
        {
            velocity.z = -epared * velocity.z;
            nuevaPos.z = minZ + radio;
        }
        else if (nuevaPos.z > maxZ - radio)
        {
            velocity.z = -epared * velocity.z;
            nuevaPos.z = maxZ - radio;
        }

        // Restricción y rebote en paredes eje X
        if (nuevaPos.x < minX + radio)
        {
            velocity.x = -epared * velocity.x;
            nuevaPos.x = minX + radio;
        }
        else if (nuevaPos.x > maxX - radio)
        {
            velocity.x = -epared * velocity.x;
            nuevaPos.x = maxX - radio;
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

        // --- Detección manual de colisión con bolas ---
        if (!gameOverActivado)
        {
            Collider[] bolasCercanas = Physics.OverlapSphere(transform.position, radioDeteccionBolas, capaBolas);
            if (bolasCercanas.Length > 0)
            {
                gameOverActivado = true;
                gameOverManager.MostrarGameOver();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza el radio de detección en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccionBolas);
    }
}
