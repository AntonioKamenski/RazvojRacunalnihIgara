using UnityEngine;

// Attached at runtime to a short-lived AoE object; deals damage once to all matching tags on trigger enter.
public class InstantAoEDamage : MonoBehaviour
{
    private DamageInfo damageInfo;
    private string targetTag;

    public void Init(DamageInfo info, string tag)
    {
        damageInfo = info;
        targetTag = tag;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;
        other.GetComponent<PlayerBase>()?.TakeDamage(damageInfo);
    }
}
