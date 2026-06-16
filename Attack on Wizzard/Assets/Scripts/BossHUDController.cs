using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHUDController : MonoBehaviour
{
    public static BossHUDController Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI bossNameText;

    private BossBase trackedBoss;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (panel != null) panel.SetActive(false);
    }

    private void Update()
    {
        if (trackedBoss == null) return;
        if (healthSlider != null)
            healthSlider.value = Mathf.Clamp01(trackedBoss.CurrentHP / trackedBoss.MaxHP);
    }

    public void Show(string bossName, BossBase boss)
    {
        trackedBoss = boss;
        if (bossNameText != null) bossNameText.text = bossName;
        if (panel != null) panel.SetActive(true);
    }

    public void Hide()
    {
        trackedBoss = null;
        if (panel != null) panel.SetActive(false);
    }
}
