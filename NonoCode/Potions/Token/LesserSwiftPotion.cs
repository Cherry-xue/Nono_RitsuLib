using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Nono.NonoCode.Potions;

public sealed class LesserSwiftPotion : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.AnyPlayer;
    //药水使用目标为任意玩家
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    //药水使用效果为抽1张牌
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        AssertValidForTargetedPotion(target);
        NCombatRoom.Instance.PlaySplashVfx(target, new Color("3d708e"));
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, target.Player);
    }
    //药水使用时，先检查目标是否有效，然后播放特效并让目标玩家抽牌
}
