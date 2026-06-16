using UnityEngine;

// Attach to an effect prefab. Fades out and self-destructs over `duration` seconds.
[RequireComponent(typeof(SpriteRenderer))]
public class TemporarySpriteEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;

    private SpriteRenderer sr;
    private Vector3 startScale;
    private float elapsed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - t);
        transform.localScale = startScale * (1f - t * 0.3f);
        if (elapsed >= duration) Destroy(gameObject);
    }
}
