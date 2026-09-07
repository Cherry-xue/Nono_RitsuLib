using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Potions;

// 超级魔力药水-可对自己使用,获得魔力
public class SuperManaPotion : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.Self;
    //药水使用目标为自己
    protected override IEnumerable<DynamicVar> CanonicalVars => [SecondaryResourceVars.For("Mana", ModResources.ManaId, 15)];
    //药水使用效果为获得15点魔力
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        await SecondaryResourceCmd.Gain(Owner, ModResources.ManaId, DynamicVars["Mana"].IntValue);
    }
    //药水使用时，先检查目标是否有效，然后让玩家获得魔力
}
