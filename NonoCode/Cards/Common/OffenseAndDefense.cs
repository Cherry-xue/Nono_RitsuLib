using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Cards;

// 攻守兼备-获得格挡并对目标造成伤害，若魔力增幅次数大于等于5，则选择一张卡牌生成到手牌中，并消耗本卡牌
public class OffenseAndDefense() : NonoCard
    (1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy,true)
//定义卡牌基本属性：1能量，攻击，普通稀有度，目标为任意敌人
{
    protected override bool ShouldGlowGoldInternal => DynamicVars["AmplificationCount"].BaseValue >= DynamicVars["Amplification"].BaseValue;
    //定义卡牌发光条件：当魔力增幅次数大于等于魔力增幅阈值时，卡牌发光
    public override bool GainsBlock => true;
    //卡牌属性：提供格挡
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new BlockVar(5, ValueProp.Move),
        new DamageVar(5, ValueProp.Move),
        new DynamicVar("AmplificationCount", 0m),
        new DynamicVar("Amplification", 5m)
    ];
    //定义可变参数:Block-格挡值，初始值为5;Damage-伤害值，初始值为5;魔力增幅次数，初始值为0;魔力增幅阈值，初始值为5
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.Choice),
        HoverTipFactory.FromCard<TurnDefenseIntoAttack>(),
        HoverTipFactory.FromCard<StrengthenDefenses>()
    ];
    //定义提示:提示抉择的相关信息,提示抉择的卡牌<让守为攻>和<强化防御>的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //如果魔力增幅大于等于5
        if (DynamicVars["AmplificationCount"].BaseValue >= DynamicVars["Amplification"].BaseValue)
        {
            CardModel cardModel;
            //创建选择的卡牌列表
            List<CardModel> cards =
            [
                CombatState.CreateCard<TurnDefenseIntoAttack>(Owner),
                CombatState.CreateCard<StrengthenDefenses>(Owner)
            ];
            //如果卡牌升级了,则将列表内的卡牌升级
            if (IsUpgraded)
            {
                foreach (var card in cards)
                {
                    CardCmd.Upgrade(card);
                }
            }
            //创建选择界面,允许玩家选择一张卡牌或跳过
            cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, Owner, canSkip: true);
            //如果玩家选择了卡牌,则将其设置为本回合免费,消耗本卡牌,并将选择的卡牌加入手牌
            if (cardModel != null)
            {
                cardModel.SetToFreeThisTurn();
                await CardCmd.Exhaust(choiceContext, cardPlay.Card);
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
                return;
            }
        }
        //否则,获得格挡并对目标造成伤害
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
    //卡牌效果:魔力增幅次数大于等于5时,选择一张卡牌生成到手牌中,并消耗本卡牌;否则,获得格挡并对目标造成伤害
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
    }
    //卡牌效果:魔力增幅次数增加1
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.Block.UpgradeValueBy(2m);
    }
    //升级效果:伤害数值增加2，格挡数值增加2
}
