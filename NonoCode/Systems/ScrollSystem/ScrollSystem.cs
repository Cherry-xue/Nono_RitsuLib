using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Nono.NonoCode.ScrollSystem;

public static class ScrollService
{
    public static async Task ScollCreate(Player Owner, CardModel card, int flag)
    {
        //创建克隆卡牌
        CardModel clonecard = card.CreateClone();
        //设置为本次战斗免费
        clonecard.SetToFreeThisCombat();
        //添加卡牌关键词：卷轴
        CardCmd.ApplyKeyword(clonecard, NonoKeywords.ScrollKeywords);
        //如果flag为0，则将克隆卡牌添加到手牌
        if (flag == 0)
        {
            await CardPileCmd.AddGeneratedCardToCombat(clonecard, PileType.Hand, Owner);
            return;
        }
        //如果flag为1，则将克隆卡牌添加到抽牌堆
        if (flag == 1)
        {
            //将克隆卡牌添加到抽牌堆
            CardPileAddResult drawResult = await CardPileCmd.AddGeneratedCardToCombat(clonecard, PileType.Draw, Owner, CardPilePosition.Random);
            CardCmd.PreviewCardPileAdd(drawResult);
            return;
        }
        //如果flag为2，则将克隆卡牌添加到弃牌堆
        if (flag == 2)
        {
            CardPileAddResult discardResult = await CardPileCmd.AddGeneratedCardToCombat(clonecard, PileType.Discard, Owner);
            CardCmd.PreviewCardPileAdd(discardResult);
            return;
        }
    }
}