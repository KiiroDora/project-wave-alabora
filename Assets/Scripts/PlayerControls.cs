using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Vector2 moveValue;  // TODO: these values are temporary, go look up an actual tutorial for rigidbody player inputs
    Rigidbody2D rb2d;

    public float moveSpeed = 2f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2d.AddForce(new Vector2(moveValue.x, 0) * moveSpeed);
    }

    private void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    private void OnAttack(InputValue value)
    {

    }

    private void OnJump(InputValue value)
    {

    }

    private void OnSprint(InputValue value)
    {

    }

    private void OnCrouch(InputValue value)
    {

    }
}
