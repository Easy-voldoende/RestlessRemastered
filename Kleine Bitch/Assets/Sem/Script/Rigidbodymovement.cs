using UnityEngine;

public class Rigidbodymovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance, groundMask);

        // Move the player based on input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 movement = moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);

        // Jump if the player is grounded and jump button is pressed
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}