using UnityEngine;

public class CharacterVisualDatabase : MonoBehaviour
{
    public static CharacterVisualDatabase Instance { get; private set; }

    [System.Serializable]
    public class ClassVisuals
    {
        [Header("Stats")]
        public float defense = 0f;

        [Header("Idle / Walk")]
        public Sprite[] idleWalkFrames;
        public float idleFps = 8f;

        [Header("Attack")]
        public Sprite[] attackFrames;
        public float attackFps = 12f;
    }

    [Header("Class Visuals")]
    public ClassVisuals warrior;
    public ClassVisuals rogue;
    public ClassVisuals mage;
    public ClassVisuals archer;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ClassVisuals GetVisuals(GameManager.PlayerClassType classType)
    {
        return classType switch
        {
            GameManager.PlayerClassType.Warrior => warrior,
            GameManager.PlayerClassType.Rogue   => rogue,
            GameManager.PlayerClassType.Mage    => mage,
            GameManager.PlayerClassType.Archer  => archer,
            _                                   => warrior
        };
    }
}
