using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI bossNameLabel;

    void Start()
    {
        if (panel != null) panel.SetActive(false);

        Boss.OnHealthChanged += UpdateBar;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBossFight += ShowBar;
            GameManager.Instance.OnWin       += HideBar;
            GameManager.Instance.OnLose      += HideBar;
        }
    }

    void OnDestroy()
    {
        Boss.OnHealthChanged -= UpdateBar;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBossFight -= ShowBar;
            GameManager.Instance.OnWin       -= HideBar;
            GameManager.Instance.OnLose      -= HideBar;
        }
    }

    private void ShowBar()
    {
        if (panel != null) panel.SetActive(true);
        if (slider != null) slider.value = 1f;
    }

    private void HideBar()
    {
        if (panel != null) panel.SetActive(false);
    }

    private void UpdateBar(float fraction)
    {
        if (slider != null) slider.value = fraction;
    }
}
