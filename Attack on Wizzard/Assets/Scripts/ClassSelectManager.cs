using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassSelectManager : MonoBehaviour
{
    [Header("Class Buttons")]
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button rogueButton;
    [SerializeField] private Button mageButton;
    [SerializeField] private Button archerButton;

    [Header("Info Panel")]
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private TextMeshProUGUI classDescriptionText;
    [SerializeField] private TextMeshProUGUI passiveDescriptionText;

    [Header("Confirm")]
    [SerializeField] private Button playButton;

    private static readonly string[] Names =
        { "Warrior", "Rogue", "Mage", "Archer" };

    private static readonly string[] Descriptions =
    {
        "Heavy melee fighter. Wide sword swings hit all nearby enemies at once.\nElement Bonus: Fire (+25% fire damage)",
        "Swift dual-slash combatant. Two rapid hits with high bleed buildup.\nElement Bonus: Shadow (+25% shadow damage)",
        "Arcane spellcaster. Fires homing ice orbs that slow enemies on hit.\nElement Bonus: Ice (+25% ice damage)",
        "Precise ranged hunter. Rapid arrows with lightning element.\nElement Bonus: Lightning (+25% lightning damage)"
    };

    private static readonly string[] Passives =
    {
        "Passive - Fortify: Take 15% less damage when below 50% HP.",
        "Passive - Evasion: 15% chance to dodge any incoming hit entirely.",
        "Passive - Chill: All attacks slow enemies by 50% for 1.5 seconds.",
        "Passive - Swiftness: Attack speed increases the longer you keep moving (up to +40%)."
    };

    private void Start()
    {
        warriorButton.onClick.AddListener(() => SelectClass(GameManager.PlayerClassType.Warrior));
        rogueButton.onClick.AddListener(  () => SelectClass(GameManager.PlayerClassType.Rogue));
        mageButton.onClick.AddListener(   () => SelectClass(GameManager.PlayerClassType.Mage));
        archerButton.onClick.AddListener( () => SelectClass(GameManager.PlayerClassType.Archer));

        playButton.onClick.AddListener(OnPlayClicked);

        SelectClass(GameManager.PlayerClassType.Warrior);
    }

    private void SelectClass(GameManager.PlayerClassType classType)
    {
        AudioManager.Instance?.PlayButtonClick();
        GameManager.Instance.SetSelectedClass(classType);

        int i = (int)classType;
        if (classNameText != null)        classNameText.text        = Names[i];
        if (classDescriptionText != null) classDescriptionText.text = Descriptions[i];
        if (passiveDescriptionText != null) passiveDescriptionText.text = Passives[i];
    }

    private void OnPlayClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
        GameManager.Instance.StartGame();
    }
}
