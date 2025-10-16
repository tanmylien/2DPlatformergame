using UnityEngine;

public class BearEnemy : EnemyController
{
    [Header("Patrol")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 1.5f;
    public float arriveDistance = 0.03f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Transform target;

    void Awake()
    {
        stompable = false; // cannot be stomped

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (rb)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.freezeRotation = true;
        }
        target = rightPoint != null ? rightPoint : leftPoint;
    }

    void FixedUpdate()
    {
        if (!leftPoint || !rightPoint || !rb) return;

        Vector3 goal = new Vector3(target.position.x, transform.position.y, transform.position.z);
        Vector3 next = Vector3.MoveTowards(transform.position, goal, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (sr) sr.flipX = (target == leftPoint);

        if (Mathf.Abs(transform.position.x - goal.x) <= arriveDistance)
            target = (target == rightPoint) ? leftPoint : rightPoint;
    }

    protected override void OnBodyHitPlayer(Player player)
    {
        GameManager.I.KillPlayer();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.collider.GetComponent<Player>();
        if (!player) return;
        HandleCollision(player, col);
    }
}
