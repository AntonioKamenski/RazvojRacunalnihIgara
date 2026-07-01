using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button difficultyButton;
    [SerializeField] private Button quitButton;

    [Header("Music Volume")]
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Music Button Sprites")]
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private Image musicIconImage;

    [Header("Button Labels")]
    [SerializeField] private TextMeshProUGUI difficultyButtonText;

    private void Start()
    {
        AudioManager.Instance?.PlayMenuMusic();

        RefreshUI();

        startButton.onClick.AddListener(OnStartClicked);
        musicToggleButton.onClick.AddListener(OnMusicToggleClicked);
        difficultyButton.onClick.AddListener(OnDifficultyClicked);
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(AudioManager.Instance != null ? AudioManager.Instance.GetMusicVolume() : 1f);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
    }

    private void OnStartClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        GameManager.Instance.GoToBossPreview();
    }

    private void OnMusicToggleClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        GameManager.Instance.ToggleMusic();
        AudioManager.Instance?.SetMusicEnabled(GameManager.Instance.IsMusicEnabled);
        UpdateMusicSprite();
    }

    private void OnDifficultyClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        GameManager.Instance.CycleDifficulty();
        UpdateDifficultyLabel();
    }

    private void OnQuitClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
    }

    private void RefreshUI()
    {
        UpdateMusicSprite();
        UpdateDifficultyLabel();
    }

    private void UpdateMusicSprite()
    {
        if (musicIconImage == null) return;
        musicIconImage.sprite = GameManager.Instance.IsMusicEnabled ? musicOnSprite : musicOffSprite;
    }

    private void UpdateDifficultyLabel()
    {
        if (difficultyButtonText == null) return;
        difficultyButtonText.text = $"Difficulty: {GameManager.Instance.CurrentDifficulty}";
    }
}
