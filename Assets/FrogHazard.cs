using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FrogHazard : EnemyController
{
    Collider2D col;
    SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr  = GetComponent<SpriteRenderer>();
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = true;
        rb.useFullKinematicContacts = false;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        var player = c.collider.GetComponent<Player>();
        if (!player) return;
        HandleCollision(player, c);
    }

    protected override void Die()
    {
        if (deathEffect) Instantiate(deathEffect, transform.position, Quaternion.identity);
        if (col) col.enabled = false;
        if (sr) sr.enabled = false;
        Destroy(gameObject, 0.2f);
    }
}
