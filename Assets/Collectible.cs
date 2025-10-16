using UnityEngine;

public enum CollectType { Cherry, Heart }

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [Header("What this gives")]
    public CollectType type = CollectType.Cherry;
    public int amount = 1;

    [Header("Feedback")]
    public GameObject pickupFxPrefab;     
    public float fxScale = 1f;            
    public Vector2 fxOffset = Vector2.zero;
    public AudioClip pickupSfx;
    [Range(0f,1f)] public float sfxVolume = 1f;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
        gameObject.tag = "Collectible";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (type == CollectType.Cherry) GameManager.I.AddCherry(amount);
        else                            GameManager.I.AddHeart(amount);

        if (pickupFxPrefab)
        {
            var pos = (Vector2)transform.position + fxOffset;
            var fx = Instantiate(pickupFxPrefab, pos, Quaternion.identity);
            fx.transform.localScale = Vector3.one * fxScale;   
        }

        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

        Destroy(gameObject);
    }
}
