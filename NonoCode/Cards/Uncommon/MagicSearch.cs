using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Nono.NonoCode.Cards;

// 魔力检索-抽取卡牌，若抽取的卡牌中包含魔法牌，则额外抽取卡牌
public sealed class MagicSearch() : NonoCard
    (1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
//定义卡牌基本属性：1能量，技能，罕见稀有度，目标为自己
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CardsVar(2),
        new DynamicVar("ExtraCards", 1m)
    ];
    //定义可变参数:抽取卡牌数值，初始值为2;额外抽取卡牌数值，初始值为1
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //卡牌关键词：魔法牌
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel cardModel;
        bool flag = false;
        for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            cardModel = await CardPileCmd.Draw(choiceContext, Owner);
            if (cardModel != null && cardModel.Keywords.Contains(NonoKeywords.MagicCard))
            {
                flag = true;
            }
        }
        if (flag)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars["ExtraCards"].BaseValue, Owner);
        }
    }
    //卡牌效果:抽取等同于DynamicVars.Cards数值的卡牌，若抽取的卡牌中包含魔法牌，则额外抽取等同于DynamicVars["ExtraCards"]数值的卡牌
    protected override void OnUpgrade()
    {
        DynamicVars["ExtraCards"].UpgradeValueBy(1m);
    }
    //升级效果：增加额外抽取的卡牌数量
}