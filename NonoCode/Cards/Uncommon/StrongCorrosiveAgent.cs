using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Cards;

// 强效腐蚀剂-使目标失去所有格挡，移除人工制品，对目标造成伤害
public class StrongCorrosiveAgent() : NonoCard
    (1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    //定义卡牌基本属性：1能量，攻击，罕见稀有度，目标为任意敌人。
{
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    //卡牌关键词：消耗
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为7
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ArtifactPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
    //定义提示：提示内容为ArtifactPower和Block的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.LoseBlock(choiceContext, cardPlay.Target, cardPlay.Target.Block, Owner.Creature);
        if (cardPlay.Target.HasPower<ArtifactPower>())
        {
            await PowerCmd.Remove<ArtifactPower>(cardPlay.Target);
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
    }
    //卡牌效果：使目标失去所有格挡，如果目标具有人工制品，则移除人工制品，对目标造成等同于DynamicVars.Damage数值的伤害
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    //升级效果:伤害数值增加3
}
