using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.Potions;

// 烈火药剂-可在战斗中使用，给予自己预燃
public class FlaskOfFire : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Common;
    //药水稀有度为普通
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.Self;
    //药水使用目标为自己
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("PreBurningPower", 6m)];
    //药水使用效果为获得预燃，数值为6
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<PreBurningPower>()];
    //药水使用时，额外提示为获得预燃
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<PreBurningPower>(choiceContext, target, DynamicVars["PreBurningPower"].BaseValue, Owner.Creature, null);
    }
    //药水使用时，判断目标有效，并应用预燃效果，数值为PreBurningPower
}
