using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Scaling")]
    [SerializeField] private int baseXPToLevel = 100;
    [SerializeField] private float xpScaling = 1.4f;

    public int   Level           { get; private set; } = 1;
    public float MagnetismRadius { get; private set; } = 0f;

    public event System.Action OnLevelUp;

    private int currentXP = 0;
    private int xpToNextLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        xpToNextLevel = baseXPToLevel;
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
            LevelUp();
        UpdateUI();
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        Level++;
        xpToNextLevel = Mathf.RoundToInt(baseXPToLevel * Mathf.Pow(xpScaling, Level - 1));
        OnLevelUp?.Invoke();
        AudioManager.Instance?.PlayLevelUp();
    }

    public void IncreaseMagnetism(float radius) => MagnetismRadius += radius;

    private void UpdateUI()
    {
        if (xpSlider  != null) xpSlider.value  = xpToNextLevel > 0 ? (float)currentXP / xpToNextLevel : 0f;
        if (levelText != null) levelText.text   = $"Lv {Level}";
    }
}
