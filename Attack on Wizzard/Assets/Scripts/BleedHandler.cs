using UnityEngine;
using UnityEngine.UI;

public interface IBleedable
{
    void TakeBleedDamage(float damage);
}

public class BleedHandler : MonoBehaviour
{
    [Header("Bleed Settings")]
    [SerializeField] private float threshold = 100f;
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private int tickCount = 5;

    [Header("UI")]
    [SerializeField] private Slider bleedBar;

    private float buildup = 0f;
    private bool hasProcd = false;
    private int remainingTicks = 0;
    private float tickTimer = 0f;

    private IBleedable owner;

    private void Awake()
    {
        owner = GetComponent<IBleedable>();

        if (bleedBar != null)
        {
            bleedBar.minValue = 0f;
            bleedBar.maxValue = 1f;
            bleedBar.value = 0f;
        }
    }

    private void Update()
    {
        if (remainingTicks <= 0) return;

        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            tickTimer = tickInterval;
            owner?.TakeBleedDamage(damagePerTick);
            remainingTicks--;
        }
    }

    public void AddBuildup(float amount)
    {
        buildup += amount;
        UpdateUI();

        if (!hasProcd && buildup >= threshold)
            Proc();
    }

    private void Proc()
    {
        hasProcd = true;
        remainingTicks = tickCount;
        tickTimer = tickInterval;
    }

    private void UpdateUI()
    {
        if (bleedBar == null) return;
        bleedBar.value = Mathf.Clamp01(buildup / threshold);
    }

    public float Buildup => buildup;
    public float Threshold => threshold;
    public bool HasProcd => hasProcd;
    public bool IsBleeding => remainingTicks > 0;
}
