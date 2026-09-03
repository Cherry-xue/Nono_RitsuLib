using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Nono.NonoCode.Potions;

public class HealingPotion : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.AnyPlayer;
    //药水使用目标为自己
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(6)];
    //药水使用效果为治疗，治疗数值等同于DynamicVars.Heal的数值，初始值为6
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        await CreatureCmd.Heal(target, DynamicVars.Heal.BaseValue);
    }
    //药水使用时，先检查目标是否有效,然后对目标进行治疗，治疗数值等同于DynamicVars.Heal的数值
}
