using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Nono.NonoCode.Cards;

// 命运纺锤-抽取一张卡牌，按照该卡牌的类型触发不同的效果
public sealed class SpindleOfFate() : NonoCard
    (1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
//定义卡牌基本属性：1能量，技能，罕见稀有度，目标为自己
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new DynamicVar("Replay", 1m),
        new DynamicVar("GigantificationPower", 1m)
    ];
    //定义可变参数:抽卡数值，初始值为1;重放数值，初始值为1;超巨化数值，初始值为1
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<GigantificationPower>(),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];
    //定义提示：提示内容为消耗和GigantificationPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card;
        int cards = (int)DynamicVars.Cards.BaseValue;
        for (int i = 0; i < cards; i++)
        {
            if (CardPile.GetCards(Owner, PileType.Hand).Count() >= 10)
            {
                return;
            }
            card = await CardPileCmd.Draw(choiceContext, Owner);
            if (card == null)
            {
                return;
            }
            if (card.Type == CardType.Attack)
            {
                await PowerCmd.Apply<GigantificationPower>(choiceContext, Owner.Creature, DynamicVars["GigantificationPower"].BaseValue, Owner.Creature, null);
                CardCmd.Preview(card);
            }
            else if (card.Type == CardType.Skill)
            {
                card.BaseReplayCount += DynamicVars["Replay"].IntValue;
                CardCmd.Preview(card);
            }
            else if (card.Type == CardType.Power)
            {
                card.SetToFreeThisTurn();
                CardCmd.Preview(card);
            }
            else if (card.Type == CardType.Status|| card.Type == CardType.Curse)
            {
                await CardCmd.Exhaust(choiceContext, card);
                cards++;
            }
            else if (card.Type == CardType.Quest)
            {
                cards++;
            }
            else
            {
                return;
            }
        }
    }
    /*卡牌效果:按照抽取的卡牌类型，分别触发不同的效果。
     * 攻击牌：获得1层超巨化;
     * 技能牌：增加1次重放次数;
     * 能力牌：本回合免费使用;
     * 状态牌或诅咒牌：消耗该卡牌并额外抽取一张卡牌;
     * 任务牌：额外抽取一张卡牌*/
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    //升级效果:增加卡牌保留效果
}