using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PigEnemy : MonoBehaviour
{
    [Header("Patrol")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 3.5f;
    public bool lockY = true;
    public float yOffset = 0f;
    public float edgePadding = 0.05f;

    [Header("Stomp / Combat")]
    public float stompOffset = 0.2f;    
    public float stompBounce = 10f;     

    [Header("FX")]
    // create a death effect when the pig is stomped and destroyed
    public GameObject deathFxPrefab;
    // make sure the death effect doesn't last too long after the pig enemy has been destroyed  
    public float fxLifetime = 0.8f;     
    public Vector2 fxOffset = Vector2.zero;

    Rigidbody2D rb;
    SpriteRenderer sr;
    float dir = 1f;
    float xMin, xMax;
    float yLockValue;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Start()
    {
        xMin = Mathf.Min(leftPoint.position.x, rightPoint.position.x);
        xMax = Mathf.Max(leftPoint.position.x, rightPoint.position.x);
        dir = (Mathf.Abs(transform.position.x - xMax) <
               Mathf.Abs(transform.position.x - xMin)) ? 1f : -1f;

        if (lockY) yLockValue = transform.position.y + yOffset;
    }

    void FixedUpdate()
    {
        float targetY = lockY ? yLockValue : transform.position.y;

        rb.velocity = new Vector2(dir * speed, 0f);
        if (lockY) rb.position = new Vector2(rb.position.x, targetY);

        if (transform.position.x >= xMax - edgePadding) dir = -1f;
        if (transform.position.x <= xMin + edgePadding) dir =  1f;

        if (sr) sr.flipX = dir < 0f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.collider.GetComponent<Player>();
        if (!player) return;

        bool playerAbove = player.transform.position.y >
                           (transform.position.y + stompOffset);
        bool fallingOrOnTop = player.velocity.y <= 0.01f;

        if (playerAbove && fallingOrOnTop)
        {
            if (player.velocity.y < stompBounce)
                player.velocity = new Vector2(player.velocity.x, stompBounce);

            Die();
        }
        else
        {
            GameManager.I.TakeDamage(1);
        }
    }

    // trigger the death effect
    void Die()
    {
        if (deathFxPrefab)
        {
            var fx = Instantiate(deathFxPrefab,
                                 (Vector2)transform.position + fxOffset,
                                 Quaternion.identity);
            if (fxLifetime > 0f) Destroy(fx, fxLifetime);
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
// the pig enemy patrols between two points
    void OnDrawGizmosSelected()
    {
        if (leftPoint && rightPoint)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(leftPoint.position, rightPoint.position);
        }
    }
#endif
}
