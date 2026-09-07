using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Nono.NonoCode.Cards;

// 哈气-对目标施加弱化和易伤效果
public class CatAngry() : NonoCard
    (0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy,true)
    //定义卡牌基本属性：0能量，技能，罕见稀有度，目标为任意敌人
{
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    //卡牌关键词：消耗
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2m),
        new PowerVar<VulnerablePower>(2m)
    ];
    //定义可变参数，分别为弱化和易伤的数值，初始值均为2
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
    ];
    //定义弱化和易伤效果的提示
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
    }
    //卡牌效果：施加等同于DynamicVars.Weak数值的弱化，施加等同于DynamicVars.Vulnerable数值的易伤
    protected override void OnUpgrade()
    {

        base.DynamicVars.Weak.UpgradeValueBy(1m);
        base.DynamicVars.Vulnerable.UpgradeValueBy(1m);
    }
    //升级效果：弱化和易伤数值均增加1
}
