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

// 火墙术-提供格挡,受到伤害时对攻击者施加燃烧
public class WallOfFire : NonoCard
{
    public WallOfFire() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    //定义卡牌基本属性：1能量，技能，罕见稀有度，目标为自己
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 2);
    }
    //定义魔力消耗为2
    public override bool GainsBlock => true;
    //卡牌属性：提供格挡
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("WallOfFire", 2m),
        new BlockVar(16m, ValueProp.Move)
    ];
    //定义可变参数：WallOfFire数值，初始值为3；Block数值，初始值为16
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    //定义提示：提示BurnPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<WallOfFirePower>(choiceContext, Owner.Creature, DynamicVars["WallOfFire"].BaseValue, Owner.Creature, this);
    }
    //卡牌效果:获得等同于DynamicVars.Block数值的格挡，并获得等同于DynamicVars["WallOfFire"]数值的WallOfFirePower
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(5m);
        DynamicVars["WallOfFire"].UpgradeValueBy(1m);
    }
    //升级效果:获得的格挡数值增加5，WallOfFire数值增加1
}
