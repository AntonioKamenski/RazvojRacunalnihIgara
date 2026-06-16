using UnityEngine;

[CreateAssetMenu(menuName = "AttackOnWizard/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        [Header("Enemies")]
        public float enemyHPMultiplier       = 1f;
        public float enemyDamageMultiplier   = 1f;

        [Header("Spawner")]
        public float spawnIntervalMultiplier = 1f;
        public float enemyCountMultiplier    = 1f;

        [Header("Player")]
        public float playerHPMultiplier      = 1f;
    }

    public Level normal    = new Level();
    public Level hard      = new Level { enemyHPMultiplier = 1.4f, enemyDamageMultiplier = 1.25f, spawnIntervalMultiplier = 0.8f, enemyCountMultiplier = 1.3f, playerHPMultiplier = 0.85f };
    public Level tarnished = new Level { enemyHPMultiplier = 2f,   enemyDamageMultiplier = 1.6f,  spawnIntervalMultiplier = 0.6f, enemyCountMultiplier = 1.6f, playerHPMultiplier = 0.7f  };

    public Level Get(GameManager.Difficulty d) => d switch
    {
        GameManager.Difficulty.Hard      => hard,
        GameManager.Difficulty.Tarnished => tarnished,
        _                                => normal
    };
}
