using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    // Credits --> Code referenced and edited from Contraband's "Basic Movement in Unity2D using the New Input System, in 6 minutes." video
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    public static float moveSpeed;
    public static float jumpForce;
    public static float fallingMultiplier = 2;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    private float horizontal;
    public static bool isKnockedback = false;


    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // set the player's horizontal velocity to input value times speed
        if (!isKnockedback) 
        {
            rb2d.linearVelocityX = horizontal * moveSpeed;
        }

        if (rb2d.linearVelocityY < 0)
        {
            rb2d.linearVelocity += (fallingMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
    }

    public bool IsGrounded()
    {
        // check if the area just below the player overlaps with any collider on the Ground layer
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        if (!spriteRenderer.flipX && horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
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

    public static IEnumerator CooldownKnockback(float time)
    {
        yield return new WaitForSeconds(time);
        isKnockedback = false;
        Debug.Log("boop");
    }

    // TODO: add sprint and crouch (could have)

}
