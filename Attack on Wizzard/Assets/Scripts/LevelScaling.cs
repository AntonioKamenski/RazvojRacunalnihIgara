using UnityEngine;

// Attach to the Player object.
// Each level-up scales every PlayerAttackBase component on the player.
public class LevelScaling : MonoBehaviour
{
    [SerializeField] private float damagePerLevel    = 0.08f; // +8% damage per level
    [SerializeField] private float attackSpeedPerLevel = 0.05f; // +5% attack speed per level

    private void Start()
    {
        if (XPManager.Instance != null)
            XPManager.Instance.OnLevelUp += ApplyLevelBonus;
    }

    private void OnDestroy()
    {
        if (XPManager.Instance != null)
            XPManager.Instance.OnLevelUp -= ApplyLevelBonus;
    }

    private void ApplyLevelBonus()
    {
        foreach (var atk in GetComponents<PlayerAttackBase>())
        {
            atk.ModifyDamage(1f + damagePerLevel);
            atk.ModifyCooldown(1f - attackSpeedPerLevel);
        }
    }
}
