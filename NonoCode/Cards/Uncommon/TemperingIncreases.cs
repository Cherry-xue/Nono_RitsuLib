using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 回火递增-伤害随魔力增幅次数递增
public class TemperingIncreases : NonoCard
{
    public TemperingIncreases() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    //定义卡牌基本属性：0能量，攻击，罕见稀有度，目标为任意敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CalculationBaseVar(7m),
        new ExtraDamageVar(3m),
        new DynamicVar("AmplificationCount", 0m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => card.DynamicVars["AmplificationCount"].BaseValue)
    ];
    //定义可变参数：伤害数值，初始值为7,魔力增幅伤害提升数值3
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //定义卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(NonoKeywords.MagicAmplification)];
    //定义提示:提示内容为魔力增幅的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
    }
    //卡牌效果:造成伤害数值等于伤害数值+魔力增幅伤害提升数值*魔力增幅次数
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
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
    //升级效果：魔力增幅伤害提升数值增加1
}