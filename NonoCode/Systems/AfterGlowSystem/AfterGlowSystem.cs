using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Nono.NonoCode.Cards;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.AfterGlowSystem;

public static class AfterGlowService
{
    public static async Task ShiftMode(PlayerChoiceContext choiceContext, Player Owner, ICombatState CombatState)
    {
        //创建选择的卡牌列表
        List<CardModel> cards =
        [
            CombatState.CreateCard<EmberStrength>(Owner),
            CombatState.CreateCard<ReignitingFlame>(Owner)
        ];
        //如果玩家拥有余烬之力,则将余烬之力卡牌升级
        if (Owner.Creature.GetPowerAmount<EmberStrengthPower>() > 0)
        {
            CardCmd.Upgrade(cards[0]);
        }
        //如果玩家拥有重燃之焰,则将重燃之焰卡牌升级
        if (Owner.Creature.GetPowerAmount<ReignitingFlamePower>() > 0)
        {
            CardCmd.Upgrade(cards[1]);
        }
        //创建选择界面,允许玩家选择一张卡牌,不允许跳过
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, Owner, canSkip: false);
        //如果玩家选择了卡牌,则自动打出该卡牌
        if (cardModel != null)
        {
            await CardCmd.AutoPlay(choiceContext, cardModel, null);
        }
    }
}
