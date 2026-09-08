using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.Powers;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards.Rare;

// 燃尽-消耗全部预燃层数,对所有敌人造成消耗的预燃层数+2次伤害
public class BurnOut : NonoCard
{
    public BurnOut() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
    //定义卡牌基本属性：1能量，攻击，罕见稀有度，目标为所有敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为5
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PreBurningPower>()
    ];
    //定义提示:提示PreBurningPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount = Owner.Creature.GetPowerAmount<PreBurningPower>() + 2;
        await PowerCmd.Remove<PreBurningPower>(Owner.Creature);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(amount).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).Execute(choiceContext);
    }
    //卡牌效果:对所有敌人造成伤害,伤害数值等同于DynamicVars.Damage数值,攻击次数等同于PreBurningPower的层数+2,并移除PreBurningPower
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    //升级效果：伤害数值增加3，能量消耗减少1
}