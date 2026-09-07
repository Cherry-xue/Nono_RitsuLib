using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 魔力屏障-获得格挡
public class ManaBlock : NonoCard
{
    public ManaBlock() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
    //定义卡牌基本属性：0能量，技能，基础稀有度，目标为自己
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move)];
    //定义可变参数：Block-格挡值，初始值为8
    public override bool GainsBlock => true;
    //定义卡牌是否获得格挡：是
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }
    //卡牌效果:获得等同于DynamicVars.Block数值的格挡
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
    //升级效果:格挡数值增加3
}