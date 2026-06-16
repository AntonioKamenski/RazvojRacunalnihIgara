using UnityEngine;

[CreateAssetMenu(menuName = "AttackOnWizard/Upgrade Definition")]
public class UpgradeDefinition : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public UpgradeType type;
    public int maxLevel = 3;
}
