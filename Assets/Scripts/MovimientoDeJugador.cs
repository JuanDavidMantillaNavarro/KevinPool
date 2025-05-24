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

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 1. Comprobar si el capsule collider está tocando el suelo
        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius + 0.05f);

        isGrounded = Physics.OverlapCapsule(point1, point2, capsuleCollider.radius * 2.5f, groundLayer).Length > 0;
        if (isGrounded && velocity.y < 0)
            velocity.y = 0f;


        // 2. Movimiento con WASD
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move += -transform.forward;
        if (Input.GetKey(KeyCode.A)) move += -transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        move = move.normalized * moveSpeed;

        // 3. Salto
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = jumpForce;

        // 4. Gravedad
        if (!isGrounded)
            velocity.y -= gravity * Time.deltaTime;
        else if (velocity.y < 0)
            velocity.y = -2f;

        // 5. Aplicar movimiento
        Vector3 finalMove = move + new Vector3(0, velocity.y, 0);
        transform.position += finalMove * Time.deltaTime;
    }

}