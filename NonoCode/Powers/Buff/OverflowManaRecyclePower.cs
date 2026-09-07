using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Powers;

// 溢能回收-打出魔法卡后获得格挡
public sealed class OverflowManaRecyclePower : NonoPower
{
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.MagicCard),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
    //定义额外的悬停提示：包含“MagicCard”关键词的卡牌和“Block”提示
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == base.Owner.Player && cardPlay.Card.Keywords.Contains(NonoKeywords.MagicCard))
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }
    //定义能力效果：在玩家使用包含“MagicCard”关键词的卡牌前，获得等同于能力数值的格挡
}
