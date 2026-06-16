using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("GameManager [Auto]");
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public enum Difficulty { Normal, Hard, Tarnished }
    public enum PlayerClassType { Warrior, Rogue, Mage, Archer }

    [SerializeField] private DifficultySettings difficultySettings;

    public Difficulty CurrentDifficulty { get; private set; } = Difficulty.Normal;
    public bool IsMusicEnabled { get; private set; } = true;
    public PlayerClassType SelectedClass { get; private set; } = PlayerClassType.Warrior;
    public BossData SelectedBoss { get; private set; }

    public DifficultySettings.Level ActiveDifficulty =>
        difficultySettings != null ? difficultySettings.Get(CurrentDifficulty) : new DifficultySettings.Level();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CycleDifficulty()
    {
        int next = ((int)CurrentDifficulty + 1) % System.Enum.GetValues(typeof(Difficulty)).Length;
        CurrentDifficulty = (Difficulty)next;
    }

    public void ToggleMusic()
    {
        IsMusicEnabled = !IsMusicEnabled;
    }

    public void SetSelectedClass(PlayerClassType classType) => SelectedClass = classType;
    public void SetSelectedBoss(BossData boss) => SelectedBoss = boss;

    public void GoToBossPreview() => SceneManager.LoadScene("BossPreview");
    public void GoToClassSelect() => SceneManager.LoadScene("ClassSelect");

    public void StartGame() => SceneManager.LoadScene("GameScene");

    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
}
