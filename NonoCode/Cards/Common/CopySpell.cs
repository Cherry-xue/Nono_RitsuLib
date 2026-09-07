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

// 抄写法术-将一张魔法卡的免费复制品加入抽牌堆
public class CopySpell : NonoCard
{
    public CopySpell() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
    //定义卡牌基本属性：0能量，技能，普通稀有度，目标为自己
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义辉星消耗为1
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.ScrollKeywords),
    ];
    //定义卷轴关键词的悬停提示
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner, filter: delegate (CardModel c)
        {
            return c != null && c.Keywords.Contains(NonoKeywords.MagicCard);
        }, source: this)).FirstOrDefault();
        if (selection != null)
        {
            //选择的卡牌不为空时，执行卷轴创建操作,1为添加进抽牌堆
            await ScrollService.ScollCreate(Owner, selection, 1);
        }
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    //升级效果:添加保留关键词
}
