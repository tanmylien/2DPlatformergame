using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;
    public float cooldown = 0.3f;   

    float lastHitTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        TryHit(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TryHit(other);
    }

    void TryHit(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - lastHitTime < cooldown) return;

        lastHitTime = Time.time;
        GameManager.I.TakeDamage(damage);
    }
}
