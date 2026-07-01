using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Pool")]
    [SerializeField] private List<UpgradeDefinition> allUpgrades;

    [Header("Timing")]
    [SerializeField] private float upgradeInterval = 60f;

    [Header("UI")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeOptionUI[] optionUIs;

    [Header("Active Upgrade Prefabs")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject daggerPrefab;
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private GameObject bladePrefab;
    [SerializeField] private GameObject poisonCloudPrefab;

    [Header("Visual Effect Prefabs")]
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private GameObject iceEffectPrefab;
    [SerializeField] private GameObject lightningEffectPrefab;

    [Header("Visual Effect Rotation Offsets")]
    [SerializeField] private float fireEffectRotationOffset = 90f;
    [SerializeField] private float iceEffectRotationOffset = 90f;
    [SerializeField] private float lightningEffectRotationOffset = 0f;

    private PlayerBase player;
    private float timer = 0f;
    private readonly Dictionary<UpgradeType, int> levels = new Dictionary<UpgradeType, int>();

    private void Start()
    {
        player = FindFirstObjectByType<PlayerBase>();
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= upgradeInterval)
        {
            timer = 0f;
            ShowUpgrades();
        }
    }

    private void ShowUpgrades()
    {
        if (upgradePanel == null || allUpgrades == null || allUpgrades.Count == 0) return;

        var choices = PickRandom(3);
        if (choices.Count == 0) return;

        upgradePanel.SetActive(true);
        Time.timeScale = 0f;

        for (int i = 0; i < optionUIs.Length; i++)
        {
            if (i < choices.Count)
            {
                optionUIs[i].gameObject.SetActive(true);
                int currentLevel = levels.TryGetValue(choices[i].type, out int lv) ? lv : 0;
                optionUIs[i].Setup(choices[i], currentLevel, this);
            }
            else
            {
                optionUIs[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectUpgrade(UpgradeDefinition upgrade)
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;

        if (!levels.ContainsKey(upgrade.type))
            levels[upgrade.type] = 0;

        levels[upgrade.type]++;
        ApplyUpgrade(upgrade, levels[upgrade.type]);
    }

    private void ApplyUpgrade(UpgradeDefinition upgrade, int newLevel)
    {
        if (player == null) return;

        if (IsPassive(upgrade.type))
        {
            ApplyPassive(upgrade.type, newLevel);
            return;
        }

        var existing = GetActiveUpgrade(upgrade.type);
        if (existing != null)
        {
            existing.LevelUp();
            return;
        }

        AddActiveUpgrade(upgrade.type);
    }

    private void ApplyPassive(UpgradeType type, int newLevel)
    {
        switch (type)
        {
            case UpgradeType.IronSkin:     player.AddMaxHP(player.MaxHP * 0.25f); break;
            case UpgradeType.Sharpness:    ApplyDamageBonus(0.2f); break;
            case UpgradeType.Resilience:   player.AddDefense(5f); break;
            case UpgradeType.Swiftness:    player.SetMoveSpeed(player.MoveSpeed * 1.15f); break;
            case UpgradeType.Rapidfire:    ApplyAttackSpeedBonus(0.25f); break;
            case UpgradeType.BleedEdge:    ApplyBleedBonus(0.3f); break;
            case UpgradeType.Magnetism:    XPManager.Instance?.IncreaseMagnetism(3f); break;
        }
    }

    private void ApplyDamageBonus(float factor)
    {
        foreach (var atk in player.GetComponents<PlayerAttackBase>())
            atk.ModifyCooldown(1f / (1f + factor));
    }

    private void ApplyAttackSpeedBonus(float factor)
    {
        foreach (var atk in player.GetComponents<PlayerAttackBase>())
            atk.ModifyCooldown(1f / (1f + factor));
    }

    private void ApplyBleedBonus(float factor)
    {
        // Bleed bonus is handled per attack — extended support can be added in PlayerAttackBase
        Debug.Log($"Bleed Edge applied: +{factor * 100}% bleed buildup");
    }

    private void AddActiveUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.ArrowVolley:
                var av = player.gameObject.AddComponent<ArrowVolleyUpgrade>();
                av.Init(arrowPrefab);
                break;
            case UpgradeType.SpinningBlades:
                var sb = player.gameObject.AddComponent<SpinningBladesUpgrade>();
                sb.Init(bladePrefab);
                break;
            case UpgradeType.ChainLightning:
                var cl = player.gameObject.AddComponent<ChainLightningUpgrade>();
                cl.Init(lightningEffectPrefab, lightningEffectRotationOffset);
                break;
            case UpgradeType.FireAura:
                var fa = player.gameObject.AddComponent<FireAuraUpgrade>();
                fa.Init(fireEffectPrefab, fireEffectRotationOffset);
                break;
            case UpgradeType.FrostNova:
                var fn = player.gameObject.AddComponent<FrostNovaUpgrade>();
                fn.Init(iceEffectPrefab, iceEffectRotationOffset);
                break;
            case UpgradeType.ShadowDaggers:
                var sd = player.gameObject.AddComponent<ShadowDaggersUpgrade>();
                sd.Init(daggerPrefab);
                break;
            case UpgradeType.PoisonCloud:
                var pc = player.gameObject.AddComponent<PoisonCloudUpgrade>();
                pc.Init(poisonCloudPrefab);
                break;
            case UpgradeType.Boomerang:
                var bm = player.gameObject.AddComponent<BoomerangUpgrade>();
                bm.Init(boomerangPrefab);
                break;
        }
    }

    private ActiveUpgradeBase GetActiveUpgrade(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.ArrowVolley    => player.GetComponent<ArrowVolleyUpgrade>(),
            UpgradeType.SpinningBlades => player.GetComponent<SpinningBladesUpgrade>(),
            UpgradeType.ChainLightning => player.GetComponent<ChainLightningUpgrade>(),
            UpgradeType.FireAura       => player.GetComponent<FireAuraUpgrade>(),
            UpgradeType.FrostNova      => player.GetComponent<FrostNovaUpgrade>(),
            UpgradeType.ShadowDaggers  => player.GetComponent<ShadowDaggersUpgrade>(),
            UpgradeType.PoisonCloud    => player.GetComponent<PoisonCloudUpgrade>(),
            UpgradeType.Boomerang      => player.GetComponent<BoomerangUpgrade>(),
            _                          => null
        };
    }

    private List<UpgradeDefinition> PickRandom(int count)
    {
        var pool = new List<UpgradeDefinition>();
        foreach (var u in allUpgrades)
        {
            int lv = levels.TryGetValue(u.type, out int v) ? v : 0;
            if (lv < u.maxLevel) pool.Add(u);
        }

        var result = new List<UpgradeDefinition>();
        while (result.Count < count && pool.Count > 0)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }

    private static bool IsPassive(UpgradeType type) =>
        type == UpgradeType.IronSkin    ||
        type == UpgradeType.Sharpness   ||
        type == UpgradeType.Resilience  ||
        type == UpgradeType.Swiftness   ||
        type == UpgradeType.Rapidfire   ||
        type == UpgradeType.BleedEdge   ||
        type == UpgradeType.Magnetism;
}
