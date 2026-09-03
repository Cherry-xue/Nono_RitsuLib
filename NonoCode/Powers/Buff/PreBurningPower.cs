using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Powers.Buff;

// 预燃,对首个目标造成伤害时,施加等同于层数的燃烧
public sealed class PreBurningPower : NonoPower
{
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    //显示BurnPower的相关信息
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature dealer, DamageResult result, ValueProp props, Creature target, CardModel cardSource)
    {
        if (dealer != null && dealer == Owner && props.IsPoweredAttack() /*&& cardSource.Keywords.Contains(NonoKeywords.MagicCard)*/)
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, target, Amount, Owner, null);
            await PowerCmd.Remove<PreBurningPower>(Owner);
        }
    }
    //造成伤害时,施加等同PreBurningPower层数的BurnPower,随后移除全部PreBurningPower
}