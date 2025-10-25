using UnityEngine;

public class BearEnemy : EnemyController
{
    [Header("Patrol")]
    // empty GameObjecrs in the scene that act as patrol boundaries
    public Transform leftPoint;
    public Transform rightPoint;
    // how fast the bear walks between two points
    public float speed = 1.5f;
    // a small tolerance so tge bear doesn't jitter when checking if it's reached the target
    public float arriveDistance = 0.03f;

    Rigidbody2D rb; //control movement using Unity physics
    SpriteRenderer sr; // used to flip the sprite so the bear faces the right way
    Transform target;

    void Awake()
    {
        stompable = false; // the bear cannot be stomped or destroyed, unlike the pig or frog

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
        // only runs if both patrol points and Rigidbody are valid
        if (!leftPoint || !rightPoint || !rb) return;

        Vector3 goal = new Vector3(target.position.x, transform.position.y, transform.position.z);
        Vector3 next = Vector3.MoveTowards(transform.position, goal, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (sr) sr.flipX = (target == leftPoint);

        if (Mathf.Abs(transform.position.x - goal.x) <= arriveDistance)
            target = (target == rightPoint) ? leftPoint : rightPoint;
    }
    // If the bear touches the player, it immediately kills them
    protected override void OnBodyHitPlayer(Player player)
    {
        GameManager.I.KillPlayer();
    }

    // Detects when the bear collides with something
    void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.collider.GetComponent<Player>();
        if (!player) return;
        HandleCollision(player, col);
    }
}
