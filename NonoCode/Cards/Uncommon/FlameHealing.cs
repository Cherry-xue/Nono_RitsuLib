using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 火焰疗愈-获得格挡，消耗全部状态牌并回血
public class FlameHealing : NonoCard
{
    public FlameHealing() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    //定义卡牌基本属性：0能量，技能，罕见稀有度，目标为自身
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 2);
    }
    //定义魔力消耗为2
    public override bool GainsBlock => true;
    //定义该卡牌可以获得格挡
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new BlockVar(4, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHeal").WithMultiplier((CardModel card, Creature _) => GetStatuses(card.Owner).Count())
    ];
    //定义可变参数：格挡数值，初始值为4
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>[HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        List<CardModel> list = GetStatuses(Owner).ToList();
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext, item);
            await CreatureCmd.Heal(Owner.Creature, 1, true);
        }
    }
    //卡牌效果:获得格挡，格挡数值等同于DynamicVars.Block的数值，之后将玩家所有非消耗堆的状态牌消耗掉，每消耗1张状态牌回复1点生命
    private static IEnumerable<CardModel> GetStatuses(Player owner)
    {
        return owner.PlayerCombatState.AllCards.Where((CardModel c) => c.Type == CardType.Status && c.Pile.Type != PileType.Exhaust);
    }
    //获取玩家所有非消耗堆的状态牌
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
    }
    //升级效果：格挡数值增加2
}
