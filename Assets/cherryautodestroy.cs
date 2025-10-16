using UnityEngine;

public class CherryAutoDestroy : MonoBehaviour
{
    public float lifeTime = 0.5f;  

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}