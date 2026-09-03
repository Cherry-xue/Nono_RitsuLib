using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Nono.NonoCode.Potions;

public class SwiftnessPotion : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.AnyPlayer;
    //药水使用目标为任意玩家
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(1m)];
    //药水使用效果为增加敏捷，增加数值等同于DynamicVars.Dexterity的数值，初始值为1
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];
    //定义魔法和格挡效果的提示
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<DexterityPower>(choiceContext, target, DynamicVars.Dexterity.BaseValue, Owner.Creature, null);
    }
    //药水使用时，先检查目标是否有效,然后对目标增加敏捷，增加数值等同于DynamicVars.Dexterity的数值
}
