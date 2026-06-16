using UnityEngine;
using TMPro;
using System.Collections;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject gameEndPanel;

    [Header("Defeat")]
    [SerializeField] private TextMeshProUGUI defeatText;

    [Header("Victory")]
    [SerializeField] private TextMeshProUGUI victoryText;

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float holdDuration   = 7f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (gameEndPanel != null) gameEndPanel.SetActive(false);
        SetAlpha(defeatText,  0f);
        SetAlpha(victoryText, 0f);
    }

    public void ShowDeath()
    {
        if (gameEndPanel != null) gameEndPanel.SetActive(true);
        Time.timeScale = 0f;
        if (defeatText != null)
            StartCoroutine(FadeInThenMenu(defeatText));
        else
            StartCoroutine(WaitThenMenu());
    }

    public void ShowVictory(string bossName)
    {
        if (gameEndPanel != null) gameEndPanel.SetActive(true);
        Time.timeScale = 0f;
        if (victoryText != null)
            StartCoroutine(FadeInThenMenu(victoryText));
        else
            StartCoroutine(WaitThenMenu());
    }

    private IEnumerator FadeInThenMenu(TextMeshProUGUI text)
    {
        Color c = text.color;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            text.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(elapsed / fadeInDuration));
            yield return null;
        }
        text.color = new Color(c.r, c.g, c.b, 1f);

        yield return new WaitForSecondsRealtime(holdDuration - fadeInDuration);

        Time.timeScale = 1f;
        GameManager.Instance?.LoadMainMenu();
    }

    private IEnumerator WaitThenMenu()
    {
        yield return new WaitForSecondsRealtime(holdDuration);
        Time.timeScale = 1f;
        GameManager.Instance?.LoadMainMenu();
    }

    private static void SetAlpha(TextMeshProUGUI text, float a)
    {
        if (text == null) return;
        var c = text.color;
        text.color = new Color(c.r, c.g, c.b, a);
    }
}
