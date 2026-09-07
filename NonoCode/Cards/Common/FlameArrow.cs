using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Cards;

// 火焰箭-对目标造成伤害
public class FlameArrow() : NonoCard
    (1,CardType.Attack, CardRarity.Common,TargetType.AnyEnemy,true)
    //定义卡牌基本属性：1能量，攻击，基础稀有度，目标为任意敌人
{
    public override int CanonicalStarCost => 1;
    //定义星辉消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为12
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
    }
    //卡牌效果：对目标造成等同于DynamicVars.Damage数值的伤害
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
    }
    //升级效果：伤害数值增加6
}
