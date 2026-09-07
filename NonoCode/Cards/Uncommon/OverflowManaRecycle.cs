using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.Cards;

// 溢能回收-每当使用魔法卡时，获得格挡
public class OverflowManaRecycle() : NonoCard
    (1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
    //定义卡牌基本属性：1能量，能力，罕见稀有度，目标为自己
{
    private const string _blockOnManaKey = "BlockOnMana";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BlockOnMana", 4m)];
    //定义可变参数：使用魔法卡获得的格挡数值，初始值为4
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.MagicCard),
        HoverTipFactory.Static(StaticHoverTip.Block),
    ];
    //定义魔法和格挡效果的提示
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<OverflowManaRecyclePower>(choiceContext, Owner.Creature, DynamicVars["BlockOnMana"].BaseValue, Owner.Creature, this);
    }
    //卡牌效果:获得一个OverflowManaRecyclePower，效果为：每当使用一张魔法卡时，获得等同于DynamicVars["BlockOnMana"]数值的格挡
    protected override void OnUpgrade()
    {

        DynamicVars["BlockOnMana"].UpgradeValueBy(1m);
    }
    //升级效果:使用魔法卡获得的格挡数值增加1
}
