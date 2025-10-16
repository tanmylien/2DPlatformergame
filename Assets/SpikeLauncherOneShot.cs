 using UnityEngine;
using System.Collections;

public class SpikeLauncherOneShot : MonoBehaviour
{
    [Header("Links")]
    public Transform spike;           

    [Header("Timing")]
    public float armDelayMin = 1f;
    public float armDelayMax = 2f;

    [Header("Launch")]
    public float launchDistance = 10f;   
    public float launchSpeed = 8f;

    [Header("After Launch")]
    public bool destroyAfterLaunch = true;
    public float destroyDelay = 0.5f;

    Vector3 startLocal;
    bool fired = false;

    void Awake()
    {
        if (!spike)
        {
            spike = transform.Find("Spike");
        }
    }

    void Start()
    {
        if (spike) startLocal = spike.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (fired) return;
        if (!other.CompareTag("Player")) return;

        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        fired = true;

        float delay = Random.Range(armDelayMin, armDelayMax);
        yield return new WaitForSeconds(delay);

        Vector3 targetLocal = startLocal + Vector3.up * launchDistance;
        float timeout = Mathf.Max(2f, launchDistance / Mathf.Max(0.01f, launchSpeed) + 1f);
        float t = 0f;

        while (Vector3.Distance(spike.localPosition, targetLocal) > 0.01f && t < timeout)
        {
            spike.localPosition = Vector3.MoveTowards(spike.localPosition, targetLocal, launchSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        var col = spike.GetComponent<Collider2D>();
        var sr  = spike.GetComponent<SpriteRenderer>();
        if (col) col.enabled = false;
        if (sr)  sr.enabled = false;

        if (destroyAfterLaunch) Destroy(gameObject, destroyDelay);
    }
}
