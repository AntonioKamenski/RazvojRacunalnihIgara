public struct DamageInfo
{
    public float Amount;
    public ElementType Element;
    public bool CausesBleed;
    public float BleedBuildup;

    public DamageInfo(float amount, ElementType element = ElementType.None, bool causesBleed = false, float bleedBuildup = 0f)
    {
        Amount = amount;
        Element = element;
        CausesBleed = causesBleed;
        BleedBuildup = bleedBuildup;
    }

    public static DamageInfo Pure(float amount) =>
        new DamageInfo(amount, ElementType.None, false, 0f);
}
