using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float acceleration = 20f;
    public float deceleration = 30f;

    [Header("Salto")]
    public float jumpForce = 7.5f;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    [Header("Agacharse")]
    public KeyCode crouchKey = KeyCode.A;   // <-- A para agacharse
    public float crouchScaleY = 0.5f;
    public float crouchLerp = 12f;

    Rigidbody rb;
    float zVelocity;
    Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        originalScale = transform.localScale;

        // Autoconectar si no lo asignaste aún
        if (!groundCheck) groundCheck = transform.Find("GroundCheck");
        if (groundMask == 0) groundMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        // Movimiento adelante/atrás con W/S (no lo cambiamos)
        float inputZ = Input.GetAxisRaw("Vertical");
        float target = inputZ * moveSpeed;
        float rate = Mathf.Abs(target) > 0.01f ? acceleration : deceleration;
        zVelocity = Mathf.MoveTowards(zVelocity, target, rate * Time.deltaTime);

        // D = Saltar
        if (Input.GetKeyDown(KeyCode.D) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        // A = Agacharse (mantener)
        bool crouching = Input.GetKey(crouchKey);
        float targetY = crouching ? crouchScaleY : 1f;
        Vector3 goal = new Vector3(originalScale.x, targetY * originalScale.y, originalScale.z);
        transform.localScale = Vector3.Lerp(transform.localScale, goal, crouchLerp * Time.deltaTime);

        if (crouching) zVelocity *= 0.6f;
    }

    void FixedUpdate()
    {
        Vector3 move = Vector3.forward * zVelocity * Time.fixedDeltaTime; // te mueves a lo largo de Z
        rb.MovePosition(rb.position + move);
    }

    bool IsGrounded()
    {
        if (!groundCheck) return false;
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
    }
}
