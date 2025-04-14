using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    private bool movingRight = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for wall collision
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            movingRight = !movingRight;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        // Check for player collision from above
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) // Player hit from above
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}