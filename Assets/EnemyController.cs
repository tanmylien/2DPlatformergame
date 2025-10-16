using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stomp Settings")]
    public bool stompable = true;        
    public float stompBounce = 10f;      
    public float stompOffset = 0.2f;     

    [Header("VFX")]
    public GameObject deathEffect;      

    protected virtual void Die()
    {
        if (deathEffect) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void OnBodyHitPlayer(Player player)
    {
        GameManager.I.TakeDamage(1);
    }

    protected bool HandleStomp(Player player, Collision2D col)
    {
        bool playerAbove = player.transform.position.y > (transform.position.y + stompOffset);
        bool fallingOrOnTop = player.velocity.y <= 0.01f;

        if (playerAbove && fallingOrOnTop && stompable)
        {
            Die();
            player.velocity = new Vector2(player.velocity.x, stompBounce);
            return true;
        }
        return false;
    }

    protected void HandleCollision(Player player, Collision2D col)
    {
        if (!HandleStomp(player, col))
        {
            OnBodyHitPlayer(player);
        }
    }
}