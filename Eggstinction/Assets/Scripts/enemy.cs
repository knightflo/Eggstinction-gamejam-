using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public LayerMask wallLayer;

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
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            movingRight = !movingRight;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}