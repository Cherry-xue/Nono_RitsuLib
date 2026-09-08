using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Nono.NonoCode.ScrollSystem;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 攥写卷轴-选择一张魔法牌并创建三张该牌的卷轴
public class WritingScroll : NonoCard
{
    public WritingScroll() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
    //定义卡牌基本属性：2能量，技能，罕见稀有度，目标为自己
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 2);
    }
    //定义魔力消耗为2
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        CardKeyword.Exhaust
    ];
    //卡牌关键词：消耗
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.ScrollKeywords),
    ];
    //定义卷轴关键词的悬停提示
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner, filter: delegate (CardModel c)
        {
            return c!= null && c.Keywords.Contains(NonoKeywords.MagicCard);
        }, source: this)).FirstOrDefault();
        if (selection != null)
        {
            await ScrollService.ScollCreate(Owner, selection, 0);
            await ScrollService.ScollCreate(Owner, selection, 1);
            await ScrollService.ScollCreate(Owner, selection, 2);
        }
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    //升级效果：能量消耗减少1
}