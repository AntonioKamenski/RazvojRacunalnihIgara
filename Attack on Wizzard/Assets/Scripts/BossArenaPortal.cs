using UnityEngine;
using System.Collections;

// Place this on a trigger collider in the GameScene.
// When the player walks in, the portal animation stops and the boss spawns nearby.
//
// Animation setup:
//   - Assign your portal PNG frames (in order) to Portal Frames.
//   - The script expects a SpriteRenderer on portalVisual (or a child of it).
//   - Frames play in a loop until the boss is triggered.
[RequireComponent(typeof(Collider2D))]
public class BossArenaPortal : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [Header("Portal Animation")]
    [SerializeField] private GameObject portalVisual;
    [SerializeField] private Sprite[] portalFrames;
    [SerializeField] private float fps = 12f;

    [Header("Reward")]
    [SerializeField] private int bonusXP = 500;

    private bool triggered = false;
    private BossBase spawnedBoss;
    private SpriteRenderer portalRenderer;
    private Coroutine animCoroutine;

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        if (portalVisual != null)
        {
            portalRenderer = portalVisual.GetComponent<SpriteRenderer>();
            if (portalRenderer == null)
                portalRenderer = portalVisual.GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void Start()
    {
        if (portalFrames != null && portalFrames.Length > 0 && portalRenderer != null)
            animCoroutine = StartCoroutine(AnimatePortal());
        AudioManager.Instance?.PlayPortalOpen();
    }

    private IEnumerator AnimatePortal()
    {
        float delay = 1f / Mathf.Max(1f, fps);
        int frame = 0;
        while (true)
        {
            portalRenderer.sprite = portalFrames[frame];
            frame = (frame + 1) % portalFrames.Length;
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        ForceSpawn();
    }

    public void ForceSpawn()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(ActivateArena());
    }

    private IEnumerator ActivateArena()
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        if (portalVisual != null) portalVisual.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        var prefabToSpawn = bossPrefab != null
            ? bossPrefab
            : GameManager.Instance?.SelectedBoss?.bossPrefab;

        if (prefabToSpawn == null) yield break;

        Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : transform.position;
        var bossGO = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        spawnedBoss = bossGO.GetComponent<BossBase>();

        if (spawnedBoss != null)
            spawnedBoss.OnDeath += HandleBossDeath;
    }

    private void HandleBossDeath(EnemyBase boss)
    {
        XPManager.Instance?.AddXP(bonusXP);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var col = GetComponent<Collider2D>();
        if (col != null) Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        if (bossSpawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(bossSpawnPoint.position, 0.5f);
        }
    }
}
