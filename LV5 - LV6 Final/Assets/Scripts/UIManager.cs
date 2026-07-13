using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button restartButtonWin;
    [SerializeField] Button restartButtonLose;

    [Header("Optional HUD")]
    [SerializeField] TextMeshProUGUI waveLabel;

    void Awake()
    {
        // Validate required references immediately so errors are obvious
        if (startScreen  == null) Debug.LogError("[UIManager] startScreen is not assigned!");
        if (winScreen    == null) Debug.LogError("[UIManager] winScreen is not assigned!");
        if (loseScreen   == null) Debug.LogError("[UIManager] loseScreen is not assigned!");
        if (startButton  == null) Debug.LogError("[UIManager] startButton is not assigned!");

        if (startScreen  != null) startScreen.SetActive(true);
        if (winScreen    != null) winScreen.SetActive(false);
        if (loseScreen   != null) loseScreen.SetActive(false);
    }

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[UIManager] GameManager not found in scene! Add a GameManager GameObject.");
            return;
        }

        // Wire buttons
        if (startButton       != null) startButton.onClick.AddListener(OnStartClicked);
        if (restartButtonWin  != null) restartButtonWin.onClick.AddListener(Restart);
        if (restartButtonLose != null) restartButtonLose.onClick.AddListener(Restart);

        // Subscribe to game events
        GameManager.Instance.OnGameStart    += OnGameStarted;
        GameManager.Instance.OnWaveComplete += OnWaveCompleted;
        GameManager.Instance.OnBossFight    += OnBossFight;
        GameManager.Instance.OnWin          += ShowWinScreen;
        GameManager.Instance.OnLose         += ShowLoseScreen;
    }

    void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGameStart    -= OnGameStarted;
        GameManager.Instance.OnWaveComplete -= OnWaveCompleted;
        GameManager.Instance.OnBossFight    -= OnBossFight;
        GameManager.Instance.OnWin          -= ShowWinScreen;
        GameManager.Instance.OnLose         -= ShowLoseScreen;
    }

    // Called by the Start button
    public void OnStartClicked()
    {
        if (GameManager.Instance == null) { Debug.LogError("[UIManager] GameManager missing!"); return; }
        GameManager.Instance.StartGame();
    }

    private void OnGameStarted()
    {
        if (startScreen != null) startScreen.SetActive(false);
        SetWaveLabel("WAVE 1");
    }

    private void OnWaveCompleted(int wave) => SetWaveLabel($"Wave {wave} Complete!");
    private void OnBossFight()             => SetWaveLabel("BOSS FIGHT!");
    private void ShowWinScreen()           { if (winScreen  != null) winScreen.SetActive(true); }
    private void ShowLoseScreen()          { if (loseScreen != null) loseScreen.SetActive(true); }

    private void SetWaveLabel(string text) { if (waveLabel != null) waveLabel.text = text; }

    private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
