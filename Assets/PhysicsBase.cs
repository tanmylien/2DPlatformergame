using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PhysicsBase : MonoBehaviour
{
    public Vector2 velocity;
    public float gravityFactor = 3f;     
    public float desiredx = 0f;          
    public float jumpSpeed = 14f;        
    public bool grounded;
    public float autoMoveX = 0f;

    const float skin = 0.01f;           
    public float maxStepHeight = 0.25f;  

    Rigidbody2D rb;
    ContactFilter2D contactFilter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;           
        rb.freezeRotation = true;

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void FixedUpdate()
    {
        Vector2 acceleration = 9.81f * gravityFactor * Vector2.down;
        velocity += acceleration * Time.fixedDeltaTime;

        velocity.x = (autoMoveX != 0f) ? autoMoveX : desiredx;

        Vector2 move = velocity * Time.fixedDeltaTime;
        MoveHorizontal(move.x);
        MoveVertical(move.y);
    }

    public void TryJump()
    {
        if (grounded) velocity.y = jumpSpeed;
    }

    void MoveHorizontal(float dx)
    {
        if (Mathf.Abs(dx) < 1e-6f) return;

        Vector2 dir = new Vector2(Mathf.Sign(dx), 0f);
        float dist = Mathf.Abs(dx) + skin;

        RaycastHit2D[] hits = new RaycastHit2D[16];
        int cnt = rb.Cast(dir, contactFilter, hits, dist);

        if (cnt > 0)
        {
            float allowed = hits[0].distance - skin;
            if (allowed > 0f) rb.position += dir * allowed;

            if (Mathf.Abs(hits[0].normal.x) > 0.3f)
            {
                RaycastHit2D[] upHits = new RaycastHit2D[8];
                int upCnt = rb.Cast(Vector2.up, contactFilter, upHits, maxStepHeight);
                if (upCnt == 0)
                {
                    rb.position += Vector2.up * maxStepHeight;
                    cnt = rb.Cast(dir, contactFilter, hits, dist);
                    if (cnt == 0)
                    {
                        rb.position += dir * Mathf.Abs(dx);
                        CollideHorizontal(null);
                        return;
                    }
                }
            }

            CollideHorizontal(hits[0].collider);
            return;
        }

        rb.position += dir * Mathf.Abs(dx);
    }

    void MoveVertical(float dy)
    {
        grounded = false;
        if (Mathf.Abs(dy) < 1e-6f) return;

        Vector2 dir = new Vector2(0f, Mathf.Sign(dy));
        float dist = Mathf.Abs(dy) + skin;

        RaycastHit2D[] hits = new RaycastHit2D[16];
        int cnt = rb.Cast(dir, contactFilter, hits, dist);

        if (cnt > 0)
        {
            float allowed = hits[0].distance - skin;
            if (allowed > 0f) rb.position += dir * allowed;

            if (dy < 0f && hits[0].normal.y > 0.3f)
            {
                grounded = true;
                velocity.y = 0f;
            }
            else if (dy > 0f)
            {
                velocity.y = 0f; 
            }

            CollideVertical(hits[0].collider);
            return;
        }

        rb.position += dir * Mathf.Abs(dy);
    }

    protected virtual void CollideHorizontal(Collider2D col) { /* default: no-op */ }
    protected virtual void CollideVertical  (Collider2D col) { /* default: no-op */ }
}