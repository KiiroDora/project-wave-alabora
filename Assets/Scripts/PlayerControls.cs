using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    // Credits --> Code referenced and edited from Contraband's "Basic Movement in Unity2D using the New Input System, in 6 minutes." video
    Rigidbody2D rb2d;

    [SerializeField] float moveSpeed = 7;
    [SerializeField] float jumpForce = 7;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    private float horizontal;


    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // set the player's horizontal velocity to input value times speed
        rb2d.linearVelocityX = horizontal * moveSpeed;
    }

    public bool IsGrounded()
    {
        // check if the area just below the player overlaps with any collider on the Ground layer
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Attack(InputValue value)
    {

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())  // jump at max force when button is pressed and held
        {
            rb2d.linearVelocityY = jumpForce;
        }

        if (context.canceled && rb2d.linearVelocity.y > 0f)  // cut jump speed if button is released early
        {
            rb2d.linearVelocityY /= 1.6f;
        }
    }

    // TODO: add sprint and crouch (could have)

}
