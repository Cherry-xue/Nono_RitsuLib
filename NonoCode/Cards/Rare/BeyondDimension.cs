using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards.Rare;

// 次元超越-获得能量和魔力,将手牌放入抽牌堆,洗牌,从抽牌堆中抽取卡牌,如果打出的是魔法牌,则减少能量消耗
public class BeyondDimension() : NonoCard
    (18, CardType.Skill, CardRarity.Rare, TargetType.Self,true)
//定义卡牌基本属性：18能量，技能，罕见稀有度，目标为自己
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CardsVar(5),
        new EnergyVar(3),
        SecondaryResourceVars.For("Mana", ModResources.ManaId, 3),
        new DynamicVar("AmplificationCount", 0m),
    ];
    //定义可变参数:抽牌数值，初始值为5；能量数值，初始值为3；魔力数值，初始值为3
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        NonoKeywords.MagicCard,
        CardKeyword.Exhaust
    ];
    //卡牌关键词：魔法牌,消耗
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.MagicAmplification)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await SecondaryResourceCmd.Gain(Owner, ModResources.ManaId, DynamicVars["Mana"].IntValue);
        await CardPileCmd.Add(PileType.Hand.GetPile(Owner).Cards, PileType.Draw);
        await CardPileCmd.Shuffle(choiceContext, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
    //卡牌效果:获得能量数值点能量，获得魔力数值点魔力，将手牌放入抽牌堆，洗牌，从抽牌堆中抽取抽牌数值张卡牌
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(NonoKeywords.MagicCard))
        {
            AddAmplificationCount();
        }
    }
    //卡牌效果:如果打出的是魔法牌,则减少2点能量消耗
    private void AddAmplificationCount()
    {
        DynamicVars["AmplificationCount"].BaseValue += 1;
        EnergyCost.AddThisCombat(-2);
    }
    //卡牌效果:减少能量消耗,减少的数值只在本次战斗中有效 
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2m);
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
    //升级效果:增加抽牌数值2点,增加获得能量数值1点
}