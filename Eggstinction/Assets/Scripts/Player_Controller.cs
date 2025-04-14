using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    public float jumpForce = 10f;
    public float speed;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    private bool Left = false;
    private bool Right = false;

    private bool CanJump = false;

    private bool Jumping = false;
    private bool Falling = false;

    [SerializeField] private Rigidbody2D rb;

    public void MoveLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Left = true;
        }
        if (ctx.canceled)
        {
            Left = false;
        }
    }
    public void MoveRight(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Right = true;
        }
        if (ctx.canceled)
        {
            Right = false;
        }
        
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (CanJump && ctx.started)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void Update()
    {
        CanJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Left)
        {
            gameObject.transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
        if (Right)
        {
            gameObject.transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        if (!CanJump && rb.linearVelocityY > 0.1f)
        {
            Jumping = true;
            Falling = false;
        }
        else if (!CanJump && rb.linearVelocityY < -0.1f)
        {
            Jumping = false;
            Falling = true;
        }
        else
        {
            Jumping = false;
            Falling = false;
        }
    }
}
