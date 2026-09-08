using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.Powers;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 火山-对随机敌人造成伤害，伤害次数等于玩家投入的魔力数，并附加灼烧效果
public class Volcano : NonoCard
{
    public Volcano() : base(0, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy, true)
    //定义卡牌基本属性：0能量，攻击，稀有稀有度，目标为随机敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, SecondaryResourceCost.X());
    }
    //定义魔力消耗为X
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        NonoKeywords.VolcanoKeywords, 
        NonoKeywords.MagicCard
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move),
        new DynamicVar("Burn", 3m)
    ];
    //定义可变参数：伤害数值，初始值为5,灼烧数值，初始值为3
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int effectValue = cardPlay.SecondaryResources().Value(ModResources.ManaId);
        await PowerCmd.Apply<VolcanoPower>(choiceContext, Owner.Creature, DynamicVars["Burn"].BaseValue, Owner.Creature, this);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(effectValue).FromCard(this, cardPlay).TargetingRandomOpponents(CombatState).Execute(choiceContext);
        await PowerCmd.Remove<VolcanoPower>(Owner.Creature);
    }
    //卡牌效果:获得一个VolcanoPower，效果为：拥有VolcanoKeywords的卡牌造成伤害时，给予敌人等同于DynamicVars["Burn"]层数的灼烧,之后对随机敌人造成等同于DynamicVars.Damage数值的伤害，伤害次数等于玩家投入的魔力数，最后移除VolcanoPower
    protected override void OnUpgrade()
    {
        DynamicVars["Burn"].UpgradeValueBy(2m);
    }
    //升级效果：灼烧数值增加2
}