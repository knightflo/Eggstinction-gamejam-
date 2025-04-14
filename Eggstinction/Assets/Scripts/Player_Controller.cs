using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    public float jumpForce = 10f;
    public float speed;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    private bool Left = false;
    private bool Right = false;
    private bool CanJump = false;

    private bool Jumping = false;
    private bool Falling = false;

    [SerializeField] private Rigidbody2D rb;
    private static Vector2 respawnPosition;

    private void Start()
    {
        if (respawnPosition == Vector2.zero)
        {
            respawnPosition = transform.position; // Set initial respawn point
        }
    }

    public void MoveLeft(InputAction.CallbackContext ctx)
    {
        Left = ctx.started;
        Left = !ctx.canceled;
    }

    public void MoveRight(InputAction.CallbackContext ctx)
    {
        Right = ctx.started;
        Right = !ctx.canceled;
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
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
        if (Right)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y >= 0.8f) // Ensure player is coming from directly above
                {
                    return; // Do nothing if landing on top
                }
                else
                {
                    Die(); // Kill player on side or bottom collision
                    return;
                }
            }
        }
    }

    private void Die()
    {
        // Create the corpse at the death location
        GameObject corpse = Instantiate(gameObject, transform.position, transform.rotation);

        Destroy(corpse.GetComponent<Player_Controller>()); // Remove movement script

        Rigidbody2D corpseRb = corpse.GetComponent<Rigidbody2D>();
        if (corpseRb != null)
        {
            corpseRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Allow gravity but prevent unwanted movement
            corpseRb.linearVelocity = Vector2.zero; // Stop corpse from moving unexpectedly
        }

        Collider2D corpseCollider = corpse.GetComponent<Collider2D>();
        if (corpseCollider != null)
        {
            corpseCollider.gameObject.layer = LayerMask.NameToLayer("Default"); // Set corpse layer
            corpseCollider.isTrigger = false; // Keep solid ground interactions
        }

        // Ignore collisions with player and enemy
        Physics2D.IgnoreLayerCollision(corpseCollider.gameObject.layer, LayerMask.NameToLayer("player"), true);
        Physics2D.IgnoreLayerCollision(corpseCollider.gameObject.layer, LayerMask.NameToLayer("enemy"), true);

        gameObject.SetActive(false); // Hide current player before respawn
        Invoke(nameof(Respawn), 0.1f); // Slight delay to prevent double corpse spawning
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
        gameObject.SetActive(true);
    }

    public static void UpdateCheckpoint(Vector2 newCheckpoint)
    {
        respawnPosition = newCheckpoint;
    }
}