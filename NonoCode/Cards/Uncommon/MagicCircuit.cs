using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.Cards;

// 魔力回路-每打出3张魔法牌，获得1点魔力
public class MagicCircuit() : NonoCard
    (1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
//定义卡牌基本属性：1能量，能力，罕见稀有度，目标为自己
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.MagicCard)
    ];
    //定义魔法效果的提示
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MagicCircuitPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
    //卡牌效果：施加1层MagicCircuitPower
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
    //升级效果：添加固有关键词
}
