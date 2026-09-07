using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Nono.NonoCode.Cards;

// 智慧之桃-抽牌直到抽到非魔法卡
public sealed class PeachofWisdom() : NonoCard
    (0, CardType.Skill, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：0能量，技能，普通稀有度，目标为自己
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, NonoKeywords.MagicCard];
    //卡牌关键词：消耗，魔法牌
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel cardModel;
        do
        {
            cardModel = await CardPileCmd.Draw(choiceContext, Owner);
        }
        while (cardModel != null && cardModel.Keywords.Contains(NonoKeywords.MagicCard) && CardPile.GetCards(Owner, PileType.Hand).Count() < 10);
    }
    //卡牌效果：抽一张牌，如果抽到的牌是魔法牌且手牌数量未满10张，则继续抽牌，直到抽到非魔法牌或手牌数量满10张为止
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
    //升级效果：移除消耗关键词
}