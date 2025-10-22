using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CrateMovement : MonoBehaviour
{
    // Designer-facing settings
    [Header("Travel (choose ONE mode)")]
    public float moveDistance = 2f;    
    public Transform topPoint; // waypoint the crate moves to at the top
    public Transform bottomPoint; // waypoint the crate moves to at the bottom

    [Header("Motion")]
    public float speed = 2f; // Movement speed in world units per second

    // Internals
    Rigidbody2D rb; // Move the crate via Rigidbody2D.MovePosition
    Vector3 startPos; // where the crate spawned (used for distance mode)
    Vector2 targetPos; // The current goal position the player is moving toward
    float lastY; // the previous frame’s Y so we can detect up/down
    public bool MovingDown { get; private set; }  // This will be true only while the crate is descending

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;             // We drive position manually; physics forces don’t push it
    }

    void Start()
    {
        startPos = transform.position;

        // If both waypoints exist, start by heading to the bottom point;
        // otherwise use simple “distance mode” (start ↔ start±moveDistance).
        if (topPoint != null && bottomPoint != null)
        {
            targetPos = bottomPoint.position;
        }
        else
        {
            targetPos = startPos + Vector3.down * moveDistance;
        }

        lastY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Current position and next position toward the target this physics tick
        Vector2 pos  = rb.position;
        Vector2 next = Vector2.MoveTowards(pos, targetPos, speed * Time.fixedDeltaTime);

        // Determine whether the player is moving downward this tick
        MovingDown = next.y < lastY - 1e-5f;
        lastY      = next.y;

        // Actually move the kinematic body
        rb.MovePosition(next);

        //if the player arrived at (or very near) the target, flip to the opposite target.
        if (Vector2.Distance(next, targetPos) < 0.01f)
        {
            if (topPoint != null && bottomPoint != null)
            {
                // if the crate is currently moving towards bottomPoint and reaches it, switch target to topPoint
                // and vice versa
                targetPos =
                    ((Vector2)targetPos == (Vector2)bottomPoint.position)
                    ? (Vector2)topPoint.position
                    : (Vector2)bottomPoint.position;
                // the crate bounces between the two points forever
            }
            else
            {
                bool atBottom = Mathf.Abs(targetPos.y - (startPos.y - moveDistance)) < 0.01f;
                targetPos = atBottom
                    ? startPos + Vector3.up   * moveDistance
                    : startPos + Vector3.down * moveDistance;
            }
        }
    }

    // When the player lands on TOP of the crate, make the player a child so they can ride with it.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;

        // Check if the player is landing on top of the crate rather than colliding from the side or bottom
        foreach (var c in col.contacts)
        {
            // check to see if the contact normal pointing upwards by at least 30%
            if (c.normal.y > 0.3f)        
            {
                col.transform.SetParent(transform);
                break;
            }
        }
    }

    // When the player steps off, stop parenting so they’re free again.
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            col.transform.SetParent(null);
    }
}