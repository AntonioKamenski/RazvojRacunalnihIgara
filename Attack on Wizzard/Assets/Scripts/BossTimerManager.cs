using UnityEngine;

// Place in GameScene. Spawns the boss portal at portalSpawnTime,
// then force-spawns the boss at bossForceSpawnTime if the player hasn't entered it yet.
public class BossTimerManager : MonoBehaviour
{
    [Header("Timing (seconds)")]
    [SerializeField] private float portalSpawnTime    = 540f; // 9 minutes
    [SerializeField] private float bossForceSpawnTime = 600f; // 10 minutes

    [Header("Portal")]
    [SerializeField] private Vector3 portalSpawnPosition = Vector3.zero;

    private float elapsed = 0f;
    private bool portalSpawned = false;
    private BossArenaPortal portalInstance;

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (!portalSpawned && elapsed >= portalSpawnTime)
        {
            portalSpawned = true;
            SpawnPortal();
        }

        if (portalInstance != null && elapsed >= bossForceSpawnTime)
            portalInstance.ForceSpawn();
    }

    private void SpawnPortal()
    {
        var portalPrefab = GameManager.Instance?.SelectedBoss?.portalPrefab;
        if (portalPrefab == null) return;
        var go = Instantiate(portalPrefab, portalSpawnPosition, Quaternion.identity);
        portalInstance = go.GetComponent<BossArenaPortal>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(portalSpawnPosition, 1f);
    }
}
