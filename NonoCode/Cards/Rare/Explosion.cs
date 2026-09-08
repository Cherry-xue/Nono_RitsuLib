using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 爆裂-消耗所有魔力,对所有敌人造成伤害,享受多倍力量,给予自己虚弱
public class Explosion : NonoCard
{
    public Explosion() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
    //定义卡牌基本属性：1能量，攻击，稀有稀有度，目标为所有敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, SecondaryResourceCost.X());
    }
    //定义魔力消耗为X
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
        new DynamicVar("ExplosionDamage", 7m),
        new DynamicVar("PowerMultiple", 2m)
        ];
    //定义可变参数：伤害数值，初始值为7；力量加成的倍率，初始值为2
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    //定义提示：提示内容为弱化和力量的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int effectValue = cardPlay.SecondaryResources().Value(ModResources.ManaId);
        decimal explosiondamage = DynamicVars["ExplosionDamage"].BaseValue * effectValue + Owner.Creature.GetPowerAmount<StrengthPower>() * DynamicVars["PowerMultiple"].BaseValue;
        await DamageCmd.Attack(explosiondamage).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, 2, Owner.Creature, this);
    }
    //卡牌效果:对所有敌人造成伤害，伤害数值等于DynamicVars.ExplosionDamage的数值乘以玩家当前的魔力,增加力量数值乘以DynamicVars.PowerMultiple的数值,之后施加2点弱化
    protected override void OnUpgrade()
    {
        DynamicVars["ExplosionDamage"].UpgradeValueBy(3m);
        DynamicVars["PowerMultiple"].UpgradeValueBy(2m);
        EnergyCost.UpgradeBy(-1);
    }
    //升级效果：伤害数值增加5，力量加成增加2，能量消耗减少1
}
