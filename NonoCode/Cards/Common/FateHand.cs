using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Nono.NonoCode.Cards;

// 命运的指引-魔力增幅:减少能量消耗,抽取卡牌
public class FateHand() : NonoCard
    (5, CardType.Skill, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：5能量，技能，普通稀有度，目标为自身
{
    protected override bool ShouldGlowGoldInternal => DynamicVars["AmplificationCount"].BaseValue >= DynamicVars["Amplification"].BaseValue;
    //定义卡牌发光条件：当魔力增幅次数大于等于魔力增幅阈值时，卡牌发光
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2),
        new DynamicVar("AmplificationCount", 0m),
        new DynamicVar("Amplification", 5m)
    ];
    //定义可变参数:抽取卡牌数值，初始值为2;魔力增幅次数，初始值为0,魔力增幅阈值，初始值为5
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        NonoKeywords.MagicCard
    ];
    //定义卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(NonoKeywords.MagicAmplification)];
    //定义提示:提示内容为魔力增幅的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
    //卡牌效果:抽取等同于DynamicVars.Cards数值的卡牌
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(NonoKeywords.MagicCard))
        {
            AddAmplificationCount();
        }
    }
    //卡牌效果：如果打出的是魔法牌,则魔力增幅次数增加1
    private void AddAmplificationCount()
    {
        DynamicVars["AmplificationCount"].BaseValue += 1;
        EnergyCost.AddThisCombat(-1);
    }
    //卡牌效果:魔力增幅次数增加1,本场战斗中能量消耗减少1
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
    //升级效果:抽取卡牌数值增加1
}