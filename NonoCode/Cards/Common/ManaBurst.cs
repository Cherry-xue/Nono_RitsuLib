using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 魔力释放-对所有敌人造成伤害，重复3次
public class ManaBurst : NonoCard
{
    public ManaBurst() : base(0, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
    //定义卡牌基本属性：0能量，攻击，基础稀有度，目标为所有敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 2);
    }
    //定义魔力消耗为2
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为4
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState)
            .WithHitCount(3)
            .Execute(choiceContext);
    }
    //卡牌效果:对所有敌人造成等同于DynamicVars.Damage数值的伤害，重复3次
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
    //升级效果:伤害数值增加1
}