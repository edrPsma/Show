namespace Combat
{
    /// <summary>
    /// 计算伤害前事件，晚于效果器
    /// </summary>
    /// <param name="damageInfo"></param>
    public delegate void EventBeforeCalDamage(ref DamageInfo damageInfo, IEntity attacker, IEntity target, int skillID);

    /// <summary>
    /// 计算伤害后事件,晚于效果器
    /// </summary>
    /// <param name="damageInfo"></param>
    public delegate void EventAfterCalDamage(ref DamageInfo damageInfo, IEntity attacker, IEntity target, int skillID);

    /// <summary>
    /// 造成伤害事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="damageValue"></param>
    public delegate void EventOnTakeDamage(IEntity source, IEntity self, long damageValue);
}
