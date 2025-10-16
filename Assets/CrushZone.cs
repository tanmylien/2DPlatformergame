using UnityEngine;

public class CrushZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        
            GameManager.I.TakeDamage(1);
            Debug.Log("Player crushed!");
        }
    }
}
