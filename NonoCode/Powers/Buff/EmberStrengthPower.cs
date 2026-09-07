using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Nono.NonoCode.Powers;

// 余烬之力-在回合开始时,消耗余晖层数,获得力量
public sealed class EmberStrengthPower : NonoPower
{
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<AfterGlowPower>()];
    //显示BurnPower的相关信息
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner))
        {
            int after_glow_amount = Owner.GetPowerAmount<AfterGlowPower>();
            int _amount = after_glow_amount / Amount;
            if (_amount > 0)
            {
                await PowerCmd.Apply<AfterGlowPower>(new ThrowingPlayerChoiceContext(), Owner, -_amount * Amount, Owner, null);
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, _amount, Owner, null);
            }
        }
    }
    //在回合开始时,施加余晖层数除以TriggerAmount的StrengthPower
}