using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 身体强化-获得力量和敏捷能力
public class BodyStrengthen : NonoCard
{
    public BodyStrengthen() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
    //定义卡牌基本属性：1能量，能力，罕见稀有度，目标为自己
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(1m),
        new PowerVar<DexterityPower>(1m)
    ];
    //定义可变参数:获得的StrengthPower数值，初始值为1；获得的DexterityPower数值，初始值为1
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
    [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
    ];
    //定义提示:提示StrengthPower和DexterityPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity.BaseValue, Owner.Creature, this);
    }
    //卡牌效果:获得等同于DynamicVars.Strength数值的StrengthPower，并获得等同于DynamicVars.Dexterity数值的DexterityPower
    protected override void OnUpgrade()
    {
        DynamicVars.Dexterity.UpgradeValueBy(1m);
        DynamicVars.Strength.UpgradeValueBy(1m);
    }
    //升级效果:获得的DexterityPower数值增加1，StrengthPower数值增加1
}
