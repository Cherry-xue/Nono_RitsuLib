using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Potions;

// 铁皮药水-可对任何玩家使用，给予格挡
public class IronskinPotion : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.AnyPlayer;
    //药水使用目标为任意玩家
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new BlockVar(8m, ValueProp.Unpowered) ];
    //药水使用效果为给予格挡，治疗数值等同于DynamicVars.BlockVar的数值，初始值为8
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [ HoverTipFactory.Static(StaticHoverTip.Block) ];
    //定义魔法和格挡效果的提示
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        await CreatureCmd.GainBlock(target, DynamicVars.Block, null);
    }
    //药水使用时，先检查目标是否有效,然后对目标进行治疗，治疗数值等同于DynamicVars.Heal的数值
}
