using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       
    public float smoothSpeed = 0.125f;

    [Header("Clamp boundaries")]
    public Vector2 minBounds;       
    public Vector2 maxBounds;     

    private void LateUpdate()
    {
        if (!target) return;
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
    }
}
