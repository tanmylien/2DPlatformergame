using UnityEngine;

// Attributes and fields
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PhysicsBase : MonoBehaviour
{
    public Vector2 velocity; // The current velocity
    public float gravityFactor = 3f; // Scales Earth gravity    
    public float desiredx = 0f;          
    public float jumpSpeed = 14f; // Vertical speed applied when the player jumps       
    public bool grounded; // Check if the player is standing on something
    // if gounded is false, the player cannot jump
    public float autoMoveX = 0f; // For moving enemies

    const float skin = 0.01f; // tiny inset to avoid sticking/penetration          
    public float maxStepHeight = 0.25f; // how high a player can "step up" a ledge

    Rigidbody2D rb;
    ContactFilter2D contactFilter;

    void Awake()
    {
        // rigid body
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;           
        rb.freezeRotation = true; // No spinning when the player hits things

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Applies gravity, updates velocity from input, calculates displacemen, and moves the object with collision checks
    void FixedUpdate()
    {
        // applies gravity
        Vector2 acceleration = 9.81f * gravityFactor * Vector2.down;
        velocity += acceleration * Time.fixedDeltaTime;

        // set horizontal velocity 
        velocity.x = (autoMoveX != 0f) ? autoMoveX : desiredx;

        // turn velocity into a movement vector
        Vector2 move = velocity * Time.fixedDeltaTime;

        // move vertically and horizontally, checking collisions
        MoveHorizontal(move.x);
        MoveVertical(move.y);
    }

    // Only set upward speed if the player is grounded
    public void TryJump()
    {
        if (grounded) velocity.y = jumpSpeed;
    }

    void MoveHorizontal(float dx)
    { 
        // check if there is any horizontal movement dx, if not, exit early
        if (Mathf.Abs(dx) < 1e-6f) return;

        // Creates a unit vector pointing left or right, depending on whether they're trying to move left or right
        Vector2 dir = new Vector2(Mathf.Sign(dx), 0f);
        float dist = Mathf.Abs(dx) + skin;

        // cast the rigid body in the horizontal direction to see if there are walls or obstacles in the way
        RaycastHit2D[] hits = new RaycastHit2D[16];
        int cnt = rb.Cast(dir, contactFilter, hits, dist);

        // if there is a hit/ collision
        if (cnt > 0)
        {
            // move the player as close as possible to the obstacle without overlapping
            float allowed = hits[0].distance - skin;
            if (allowed > 0f) rb.position += dir * allowed;

            // If the collision surface is steep (like a wall), attempt to "step up"
            // if it's a small ledge
            if (Mathf.Abs(hits[0].normal.x) > 0.3f)
            {
                // Try to step up if the obstacle is small enough (like a ledge)
                RaycastHit2D[] upHits = new RaycastHit2D[8];
                int upCnt = rb.Cast(Vector2.up, contactFilter, upHits, maxStepHeight);

                // if there is space above, step upward and try moving forward again
                if (upCnt == 0)
                {
                    rb.position += Vector2.up * maxStepHeight;
                    cnt = rb.Cast(dir, contactFilter, hits, dist);

                    // if no obstacle after stepping up, move forward fully
                    if (cnt == 0)
                    {
                        rb.position += dir * Mathf.Abs(dx);
                        CollideHorizontal(null);
                        return;
                    }
                }
            }

            // if still blocked, stop movement and notify about the collision
            CollideHorizontal(hits[0].collider);
            return;
        }

        // if no collision at all, move freely in the desired direction
        rb.position += dir * Mathf.Abs(dx);
    }

    void MoveVertical(float dy)
    {
        // Recompute grounded based on what we hit this frame
        grounded = false;

        // if there is no vertical movement, exit early
        if (Mathf.Abs(dy) < 1e-6f) return;

        // create a unit direction vector (down = -1, up = +1)
        // and compute the distance to move with a small "skin"
        // to avoid overlap
        Vector2 dir = new Vector2(0f, Mathf.Sign(dy));
        float dist = Mathf.Abs(dy) + skin;

        // cast the rigid body in the vertical direction to check for collisions
        RaycastHit2D[] hits = new RaycastHit2D[16];
        int cnt = rb.Cast(dir, contactFilter, hits, dist);

        // if a collision is detected
        if (cnt > 0)
        {
            // move as close to the obstacle as possible without penetrating it
            float allowed = hits[0].distance - skin;
            if (allowed > 0f) rb.position += dir * allowed;

            // if the player was moving downward and hit an upward facing surface, player is on the ground
            if (dy < 0f && hits[0].normal.y > 0.3f)
            {
                grounded = true; // mark grounded for jump logic
                velocity.y = 0f; // cancel downward velocifty so the player doesn't keep falling
            }

            // if the player was moving upward, cancel upward velocity when player hits a ceiling
            else if (dy > 0f)
            {
                velocity.y = 0f; 
            }

            // Notify subclasses about the vertical collision 
            CollideVertical(hits[0].collider);
            return;
        }

        // if no collision, free movement by the requested amount
        rb.position += dir * Mathf.Abs(dy);
    }

    protected virtual void CollideHorizontal(Collider2D col) { /* default: no-op */ }
    protected virtual void CollideVertical  (Collider2D col) { /* default: no-op */ }
}