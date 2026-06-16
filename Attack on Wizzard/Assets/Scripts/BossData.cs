using UnityEngine;

[CreateAssetMenu(menuName = "AttackOnWizard/Boss Data")]
public class BossData : ScriptableObject
{
    public string bossName;
    [TextArea] public string description;
    public Sprite bossImage;
    public ElementType weakElement;
    public GameObject bossPrefab;
    public GameObject portalPrefab;
}
