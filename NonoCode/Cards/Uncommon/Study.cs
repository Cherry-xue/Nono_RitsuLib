using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 研读-抽取卡牌,当魔力增幅次数大于等于5时,触发抉择效果
public class Study : NonoCard
{
    public Study() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    //定义卡牌基本属性：1能量，技能，罕见稀有度，目标为自身
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override bool ShouldGlowGoldInternal => DynamicVars["AmplificationCount"].BaseValue >= DynamicVars["Amplification"].BaseValue;
    //定义卡牌发光条件：当魔力增幅次数大于等于魔力增幅阈值时，卡牌发光
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2),
        new DynamicVar("AmplificationCount", 0m),
        new DynamicVar("Amplification", 5m)
    ];
    //定义可变参数:抽取卡牌数,初始值为2;魔力增幅次数，初始值为0;魔力增幅阈值，初始值为5
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.Choice)
    ];
    //定义提示:提示抉择的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        //如果魔力增幅大于等于5
        if (DynamicVars["AmplificationCount"].BaseValue >= DynamicVars["Amplification"].BaseValue)
        {
            CardModel cardModel;
            //创建选择的卡牌列表
            List<CardModel> cards =
            [
                CombatState.CreateCard<QuickNotes>(Owner),
                CombatState.CreateCard<DeepRead>(Owner)
            ];
            //创建选择界面,允许玩家选择一张卡牌或跳过
            cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, Owner, canSkip: true);
            //如果玩家选择了卡牌,则自动打出该卡牌,并将其从战斗中移除
            if (cardModel != null)
            {
                await CardCmd.AutoPlay(choiceContext, cardModel, null);
                await CardPileCmd.RemoveFromCombat(cardModel, skipVisuals: false);
            }
        }
    }
    //卡牌效果:抽取等同于DynamicVars.Cards数值的卡牌,如果魔力增幅次数大于等于5,则选择打出一张QuickNotes或DeepRead卡牌
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
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
    //升级效果:增加抽取卡牌数1
}