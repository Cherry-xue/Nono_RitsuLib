using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Nono.NonoCode.Cards;

// 精读-抽取卡牌
public class DeepRead() : NonoCard
    (0, CardType.Skill, CardRarity.Token, TargetType.Self,false)
//定义卡牌基本属性：0能量，技能，Token稀有度，目标为自身,不在图鉴中显示
{
    public override bool CanBeGeneratedInCombat => false;
    //定义不能在战斗中生成
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];
    //定义可变参数:抽取卡牌数,初始值为3
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
    //卡牌效果:抽取等同于DynamicVars.Cards数值的卡牌
}