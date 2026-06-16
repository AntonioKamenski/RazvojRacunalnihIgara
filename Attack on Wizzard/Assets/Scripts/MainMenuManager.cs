using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button difficultyButton;

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
