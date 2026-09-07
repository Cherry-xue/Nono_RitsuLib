using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;
using Nono.NonoCode.AfterGlowSystem;

namespace Nono.NonoCode.Cards;

// 调谐-抽取卡牌并切换余晖模态
public class Attune() : NonoCard
    (1, CardType.Power, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：1能量，能力，普通稀有度，目标为自身
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
    ];
    //定义可变参数:抽取卡牌数值，初始值为1
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
    [
        HoverTipFactory.FromKeyword(NonoKeywords.Choice),
        HoverTipFactory.FromPower<EmberStrengthPower>(),
        HoverTipFactory.FromPower<ReignitingFlamePower>()
    ];
    //定义提示:提示内容为选择,EmberStrengthPower,ReignitingFlamePower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await AfterGlowService.ShiftMode(choiceContext, Owner, CombatState);
    }
    //卡牌效果:抽取等同于DynamicVars.Cards数值的卡牌,并切换余晖模态
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        AddKeyword(CardKeyword.Innate);
    }
    //升级效果:减少1点能量消耗,并获得固有
}