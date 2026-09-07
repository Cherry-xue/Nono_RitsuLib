using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Cards;

// 让守为攻-对目标造成伤害
public class TurnDefenseIntoAttack() : NonoCard
    (1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy,false)
//定义卡牌基本属性：1能量，攻击，Token稀有度，目标为任意敌人,不在图鉴中显示
{
    public override bool CanBeGeneratedInCombat => false;
    //定义不能在战斗中生成
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(16, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为16

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
    }
    //卡牌效果：对目标造成等同于DynamicVars.Damage数值的伤害
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
    //升级效果：伤害数值增加5
}
