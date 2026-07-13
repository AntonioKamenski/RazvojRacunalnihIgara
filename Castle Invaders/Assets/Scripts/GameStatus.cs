using UnityEngine;
using TMPro;

public class GameStatus : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] int pointsPerBlock = 50;
    [SerializeField] int pointsPerKnight = 300;
    [SerializeField] int currentScore = 0;

    private void Awake()
    {
        int gameStatusCount = Object.FindAnyObjectByType<GameStatus>().GetComponents<GameStatus>().Length;
        if (gameStatusCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        scoreText.text = currentScore.ToString();
    }
}