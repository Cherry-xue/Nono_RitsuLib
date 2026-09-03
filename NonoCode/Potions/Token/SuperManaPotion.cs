using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Nono.NonoCode.Potions;

public class SuperManaPotion : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.Self;
    //药水使用目标为自己
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StarsVar(15)];
    //药水使用效果为获得15点辉星
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    }
    //药水使用时，先检查目标是否有效，然后让玩家获得辉星
}
