using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CrateMovement : MonoBehaviour
{
    [Header("Travel (choose ONE mode)")]
    public float moveDistance = 2f;         
    public Transform topPoint;              
    public Transform bottomPoint;         

    [Header("Motion")]
    public float speed = 2f;                

    Rigidbody2D rb;
    Vector3 startPos;
    Vector2 targetPos;
    float lastY;
    public bool MovingDown { get; private set; }  

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Start()
    {
        startPos = transform.position;

        if (topPoint != null && bottomPoint != null)
        {
            targetPos = bottomPoint.position;
        }
        else
        {
            // distance mode
            targetPos = startPos + Vector3.down * moveDistance;
        }

        lastY = transform.position.y;
    }

    void FixedUpdate()
    {
        Vector2 pos = rb.position;
        Vector2 next = Vector2.MoveTowards(pos, targetPos, speed * Time.fixedDeltaTime);

        MovingDown = next.y < lastY - 1e-5f;
        lastY = next.y;

        rb.MovePosition(next);

        if (Vector2.Distance(next, targetPos) < 0.01f)
        {
            if (topPoint != null && bottomPoint != null)
            {
                targetPos = (Vector2)targetPos == (Vector2)bottomPoint.position
                          ? (Vector2)topPoint.position
                          : (Vector2)bottomPoint.position;
            }
            else
            {
                targetPos = (Mathf.Abs(targetPos.y - (startPos.y - moveDistance)) < 0.01f)
                          ? startPos + Vector3.up * moveDistance
                          : startPos + Vector3.down * moveDistance;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;
        foreach (var c in col.contacts)
        {
            if (c.normal.y > 0.3f) { col.transform.SetParent(transform); break; }
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            col.transform.SetParent(null);
    }
}