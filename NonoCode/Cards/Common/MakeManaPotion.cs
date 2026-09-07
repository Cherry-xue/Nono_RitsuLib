using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.Cards;

// 魔力药水-制作魔力药水
public class MakeManaPotion() : NonoCard
    (1, CardType.Skill, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：1能量，技能，普通稀有度，目标为自身。
{
    public override List<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        NonoKeywords.PotionMaking,
    ];
    //卡牌关键词：消耗,药水制作
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPotion<LesserManaPotion>(),
        HoverTipFactory.FromPotion<ManaPotion>()
    ];
    //定义提示:提示内容为LesserManaPotion和ManaPotion的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await PotionCmd.TryToProcure<ManaPotion>(Owner);
        }
        else
        {
            await PotionCmd.TryToProcure<LesserManaPotion>(Owner);
            await PotionCmd.TryToProcure<LesserManaPotion>(Owner);
        }
    }
    //卡牌效果:如果卡牌已升级，尝试获得ManaPotion，否则尝试获得两个LesserManaPotion
}
