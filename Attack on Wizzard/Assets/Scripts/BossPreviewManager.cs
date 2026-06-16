using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class BossPreviewManager : MonoBehaviour
{
    [Header("Boss Pool")]
    [SerializeField] private BossData[] bosses;

    [Header("UI")]
    [SerializeField] private Image bossImage;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI weakElementText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Timing")]
    [SerializeField] private float displayDuration = 7f;

    private void Start()
    {
        AudioManager.Instance?.PlayMenuMusic();

        if (bosses == null || bosses.Length == 0) return;

        var selected = bosses[Random.Range(0, bosses.Length)];
        GameManager.Instance.SetSelectedBoss(selected);
        DisplayBoss(selected);

        StartCoroutine(AutoContinue());
    }

    private IEnumerator AutoContinue()
    {
        yield return new WaitForSeconds(displayDuration);
        GameManager.Instance.GoToClassSelect();
    }

    private void DisplayBoss(BossData data)
    {
        if (bossImage != null)
        {
            bossImage.sprite = data.bossImage;
            bossImage.enabled = data.bossImage != null;
        }

        if (bossNameText != null)
            bossNameText.text = data.bossName;

        if (weakElementText != null)
        {
            weakElementText.text = $"Weak to: {data.weakElement}";
            weakElementText.color = ElementColor(data.weakElement);
        }

        if (descriptionText != null)
            descriptionText.text = data.description;
    }

    private static Color ElementColor(ElementType element) => element switch
    {
        ElementType.Fire      => new Color(1f,   0.4f, 0f),
        ElementType.Ice       => new Color(0.4f, 0.9f, 1f),
        ElementType.Lightning => new Color(1f,   0.95f, 0f),
        ElementType.Shadow    => new Color(0.6f, 0.2f, 0.9f),
        ElementType.Bleed     => new Color(0.8f, 0.1f, 0.1f),
        _                     => Color.white
    };
}
