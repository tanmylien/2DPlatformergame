using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("Follow")]
    public Transform target;          
    public float lookAhead = 2f;    
    public float verticalOffset = 0.5f;
    public Vector2 smoothTime = new Vector2(0.15f, 0.15f);

    [Header("Level Bounds")]
    public bool useBounds = true;
    public BoxCollider2D boundsCollider; 

    Vector2 _vel;          
    Vector2 _min, _max;     
    Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        if (boundsCollider)
        {
            var b = boundsCollider.bounds;
            _min = b.min;
            _max = b.max;
        }
    }

    void LateUpdate()
    {
        if (!target) return;
        float dir = Mathf.Sign(target.localScale.x);
        if (dir == 0) dir = 1;

        Vector3 desired = new Vector3(
            target.position.x + lookAhead * dir,
            target.position.y + verticalOffset,
            transform.position.z
        );

        float x = Mathf.SmoothDamp(transform.position.x, desired.x, ref _vel.x, smoothTime.x);
        float y = Mathf.SmoothDamp(transform.position.y, desired.y, ref _vel.y, smoothTime.y);
        Vector3 pos = new Vector3(x, y, desired.z);
        if (useBounds)
        {
            float halfH = _cam.orthographicSize;
            float halfW = halfH * _cam.aspect;

            pos.x = Mathf.Clamp(pos.x, _min.x + halfW, _max.x - halfW);
            pos.y = Mathf.Clamp(pos.y, _min.y + halfH, _max.y - halfH);
        }

        transform.position = pos;
    }
}
