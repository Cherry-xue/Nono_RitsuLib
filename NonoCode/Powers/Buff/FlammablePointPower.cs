using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Nono.NonoCode.Powers;

// 可燃点-在回合开始时,获得预燃
public sealed class FlammablePointPower : NonoPower
{
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PreBurningPower>(),
    ];
    //显示PreBurningPower的相关信息
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner))
        {
            await PowerCmd.Apply<PreBurningPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        }
    }
    //在回合开始时,施加等同FlammablePointPower层数的PreBurningPower
}
