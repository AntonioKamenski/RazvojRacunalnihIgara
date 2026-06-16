using UnityEngine;
using TMPro;

public class GameStatsTracker : MonoBehaviour
{
    public static GameStatsTracker Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;

    [Header("Prefabs (assign once here, shared by all enemies)")]
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject xpPickupPrefab;

    public static GameObject DamagePopupPrefab => Instance != null ? Instance.damagePopupPrefab : null;
    public static GameObject XPPickupPrefab    => Instance != null ? Instance.xpPickupPrefab    : null;

    private float elapsed = 0f;
    private int kills = 0;
    private float totalDamage = 0f;

    public float TimeAlive    => elapsed;
    public int   KillCount    => kills;
    public float TotalDamage  => totalDamage;

    public string FormattedTime
    {
        get
        {
            int m = Mathf.FloorToInt(elapsed / 60f);
            int s = Mathf.FloorToInt(elapsed % 60f);
            return $"{m:00}:{s:00}";
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (timerText    != null) timerText.text    = FormattedTime;
        if (killCountText != null) killCountText.text = $"Kills: {kills}";
    }

    public void AddKill()           => kills++;
    public void AddDamage(float d)  => totalDamage += d;
}
