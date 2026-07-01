using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class EscapeMenuManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject menuPanel;

    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button changeMusicButton;
    [SerializeField] private Button musicToggleButton;

    [Header("Music Volume")]
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Music Toggle Sprites")]
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private Image musicIconImage;

    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI changeMusicText;

    private bool isOpen = false;

    private void Start()
    {
        menuPanel.SetActive(false);

        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        changeMusicButton.onClick.AddListener(OnChangeMusicClicked);
        musicToggleButton.onClick.AddListener(OnMusicToggleClicked);

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(AudioManager.Instance != null ? AudioManager.Instance.GetMusicVolume() : 1f);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            SetMenuOpen(!isOpen);
    }

    private void SetMenuOpen(bool open)
    {
        isOpen = open;
        menuPanel.SetActive(open);
        Time.timeScale = open ? 0f : 1f;

        if (open)
            RefreshUI();
    }

    private void OnMainMenuClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        Time.timeScale = 1f;
        GameManager.Instance?.LoadMainMenu();
    }

    private void OnChangeMusicClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        AudioManager.Instance?.NextTrack();
        UpdateChangeMusicLabel();
    }

    private void OnMusicToggleClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        if (GameManager.Instance == null) return;
        GameManager.Instance.ToggleMusic();
        AudioManager.Instance?.SetMusicEnabled(GameManager.Instance.IsMusicEnabled);
        UpdateMusicIcon();
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
    }

    private void RefreshUI()
    {
        UpdateChangeMusicLabel();
        UpdateMusicIcon();
    }

    private void UpdateChangeMusicLabel()
    {
        if (changeMusicText == null) return;
        string trackName = AudioManager.Instance != null ? AudioManager.Instance.GetCurrentTrackName() : "None";
        changeMusicText.text = $"Track: {trackName}";
    }

    private void UpdateMusicIcon()
    {
        if (musicIconImage == null || GameManager.Instance == null) return;
        musicIconImage.sprite = GameManager.Instance.IsMusicEnabled ? musicOnSprite : musicOffSprite;
    }
}
