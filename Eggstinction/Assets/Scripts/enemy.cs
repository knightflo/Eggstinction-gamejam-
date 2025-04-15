using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    public Transform leftDetector;
    public Transform rightDetector;

    private Transform leftbackup;
    private Transform rightbackup;


    public float detectorRadius = 0.1f;

    private bool movingRight = true;
    private Rigidbody2D rb;

    [SerializeField] private int _scoreOnDeath = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftbackup = leftDetector;
        rightbackup = rightDetector;
    }

    private void Update()
    {
        CheckForWalls();

        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);
    }

    private void CheckForWalls()
    {
        bool wallOnRight = Physics2D.OverlapCircle(rightDetector.position, detectorRadius, wallLayer);
        bool wallOnLeft = Physics2D.OverlapCircle(leftDetector.position, detectorRadius, wallLayer);

        if (wallOnLeft)
        {
            movingRight = true;
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (wallOnRight)
        {
            movingRight = false;
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        leftDetector = rightbackup;
        rightDetector = leftbackup;

        leftbackup = leftDetector;
        rightbackup = rightDetector;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for player collision from above
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) // Player hit from above
                {
                    ScoreManager.Instance.AddScore(_scoreOnDeath);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (leftDetector != null)
            Gizmos.DrawWireSphere(leftDetector.position, detectorRadius);
        if (rightDetector != null)
            Gizmos.DrawWireSphere(rightDetector.position, detectorRadius);
    }
}
