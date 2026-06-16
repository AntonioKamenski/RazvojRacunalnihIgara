using UnityEngine;

public abstract class ActiveUpgradeBase : MonoBehaviour
{
    protected int level = 1;
    protected PlayerBase playerBase;

    protected virtual void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
    }

    public void LevelUp()
    {
        level++;
        OnLevelUp();
    }

    protected virtual void OnLevelUp() { }

    protected DamageInfo BuildDamage(float amount, ElementType element,
                                     bool causesBleed = false, float bleedBuildup = 0f)
    {
        var info = new DamageInfo(amount, element, causesBleed, bleedBuildup);
        return playerBase != null ? playerBase.ApplyElementBonus(info) : info;
    }

    protected EnemyBase FindNearestEnemy(Vector2 origin, float maxRange = float.MaxValue)
    {
        EnemyBase nearest = null;
        float nearestDist = maxRange;

        foreach (var e in FindObjectsByType<EnemyBase>(FindObjectsSortMode.None))
        {
            if (e.IsDead) continue;
            float d = Vector2.Distance(origin, e.transform.position);
            if (d < nearestDist) { nearestDist = d; nearest = e; }
        }
        return nearest;
    }

    public int Level => level;
}
