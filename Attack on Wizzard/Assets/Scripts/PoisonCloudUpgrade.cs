using UnityEngine;

public class PoisonCloudUpgrade : ActiveUpgradeBase
{
    private GameObject cloudPrefab;
    private float cooldown = 4f;
    private float timer = 0f;
    private float cloudDamage = 6f;
    private float cloudDuration = 3f;
    private float cloudRadius = 4f;

    public void Init(GameObject prefab) => cloudPrefab = prefab;

    protected override void OnLevelUp()
    {
        cloudDamage += 3f;
        cloudRadius += 0.3f;
        cooldown = Mathf.Max(1.5f, cooldown - 0.7f);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = cooldown;
        SpawnCloud();
    }

    private void SpawnCloud()
    {
        if (cloudPrefab == null) return;

        var cloud = Instantiate(cloudPrefab, transform.position, Quaternion.identity);
        var effect = cloud.GetComponent<PoisonCloudEffect>();
        if (effect != null)
            effect.Init(cloudDamage, cloudRadius, cloudDuration,
                        BuildDamage(cloudDamage, ElementType.Shadow, false, 0f));
    }
}
