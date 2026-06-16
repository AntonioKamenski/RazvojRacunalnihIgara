using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOptionUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI levelText;

    private UpgradeDefinition upgradeData;
    private UpgradeManager manager;

    public void Setup(UpgradeDefinition data, int currentLevel, UpgradeManager mgr)
    {
        upgradeData = data;
        manager = mgr;

        if (iconImage != null)    iconImage.sprite   = data.icon;
        if (nameText != null)     nameText.text      = data.upgradeName;
        if (descriptionText != null) descriptionText.text = data.description;
        if (levelText != null)    levelText.text     = currentLevel >= data.maxLevel
                                                        ? "MAX"
                                                        : $"Lv {currentLevel} → {currentLevel + 1}";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonClick();
            manager.SelectUpgrade(upgradeData);
        });
    }
}
