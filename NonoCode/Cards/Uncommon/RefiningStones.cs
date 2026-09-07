using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Potions;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.Cards;

// 炼制石头-制作药水形状的石头或黑曜石
public class RefiningStones() : NonoCard
    (1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    //定义卡牌基本属性：1能量，技能，罕见稀有度，目标为自身。
{
    public override List<CardKeyword> CanonicalKeywords => 
    [
        CardKeyword.Exhaust,
        NonoKeywords.PotionMaking,
    ];
    //卡牌关键词：消耗,药水制作
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPotion<PotionShapedRock>(),
        HoverTipFactory.FromPotion<PotionShapedObsidian>()
    ];
    //定义提示：提示内容为PotionShapedRock和PotionShapedObsidian的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await PotionCmd.TryToProcure<PotionShapedObsidian>(Owner);
        }
        else 
        {
            await PotionCmd.TryToProcure<PotionShapedRock>(Owner);
        }
    }
    //卡牌效果：如果卡牌已升级，尝试获得PotionShapedObsidian，否则尝试获得PotionShapedRock
}
