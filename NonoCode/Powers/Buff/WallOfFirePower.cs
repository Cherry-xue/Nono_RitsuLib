using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Powers;

// 火墙术-受到伤害时,对伤害来源施加等同于层数的燃烧
public sealed class WallOfFirePower : NonoPower
{
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    //显示BurnPower的相关信息
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult _, ValueProp props, Creature dealer, CardModel __)
    {
        if (target == Owner && dealer != null && props.IsPoweredAttack())
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, dealer, Amount, Owner, null);
        }
    }
    //当拥有者受到伤害时,如果伤害来源不为空且伤害类型为攻击,则对伤害来源施加等同于WallofFirePower数值的BurnPower
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner.Side != side)
        {
            await PowerCmd.Remove(this);
        }
    }
    //在对方回合结束时,移除WallofFirePower
}