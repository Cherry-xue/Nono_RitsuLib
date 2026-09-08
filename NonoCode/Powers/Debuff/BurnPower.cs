using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.HealthBars;

namespace Nono.NonoCode.Powers;

// 燃烧-在回合开始时,受到伤害,并减少层数
public sealed class BurnPower : NonoPower, IHealthBarForecastSource
{
    public override PowerType Type => PowerType.Debuff;
    //定义能力类型：减益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("RealDamage", 2m)
    ];
    public void SetDamage()
    {
        AssertMutable();
        DynamicVars["RealDamage"].BaseValue = Amount * 2m;
    }
    //定义一个方法来设置伤害数值，伤害数值为能力数值的两倍
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return HealthBarForecasts.Single(
            Amount * 2, // 展示的数量
            new Color(1f, 0.7843f, 0.1961f, 1f), // 颜色
            HealthBarForecastGrowthDirection.FromRight // 从左边开始延伸还是右边开始
        // 0, // 顺序，越大越远离血条边缘，默认0
        // PreloadManager.Cache.GetMaterial("res://xxx.tres") // 如果需要自定义材质
        );
    }
    //定义血条预测：根据能力数值预测即将受到的伤害，颜色为橙色
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
    {
        SetDamage();
        return base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }
    //在能力数值改变后调用SetDamage方法来更新伤害数值
    public int CalculateTotalDamageNextTurn()
    {
        decimal num = default;
        int num2 = Amount;
        for (int i = 0; i < num2; i++)
        {
            decimal damage = Amount * 2 - i;
            damage = Hook.ModifyDamage(Owner.CombatState.RunState, Owner.CombatState, Owner, null, damage, ValueProp.Unblockable | ValueProp.Unpowered, null, null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
            num += damage;
        }
        return (int)num;
    }
    //定义一个方法来计算下回合即将受到的总伤害，考虑到能力数值的递减和可能的伤害修改
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner, Amount * 2, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        if (Owner.IsAlive)
        {
            decimal cost = Math.Max(Amount / 2, 1m) * -1m;
            await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), this, cost, null, null, false);
        }
        else
        {
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    }
}