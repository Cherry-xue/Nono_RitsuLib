using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 神化之书-对目标造成伤害，若目标死亡且触发了致命效果，则随机升级一张可升级的卡牌
public class DivinationBook : NonoCard
{
    public DivinationBook() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
    //定义卡牌基本属性：1能量，攻击，稀有稀有度，目标为任意敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1  
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为10
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
        CardKeyword.Exhaust
    ];
    //卡牌关键词：消耗
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        bool shouldTriggerFatal = cardPlay.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
        if (shouldTriggerFatal && attackCommand.Results.SelectMany((List<DamageResult> r) => r).Any((DamageResult r) => r.WasTargetKilled))
        {
            var upgradableCards = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsUpgradable).ToList();
            if (upgradableCards.Count > 0)
            {
                await Cmd.Wait(0.5f);
                var cardModel = Owner.RunState.Rng.Niche.NextItem(upgradableCards);
                if (cardModel == null) return;
                Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(Owner.NetId).UpgradedCards.Add(cardModel.Id);
                cardModel.UpgradeInternal();
                cardModel.FinalizeUpgradeInternal();
                if (LocalContext.IsMe(Owner))
                    NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(NCardSmithVfx.Create([
                        cardModel
                    ])!);
            }
        }
    }
    //卡牌效果:对目标造成伤害，伤害数值为10，若目标死亡且触发了致命效果，则随机升级一张可升级的卡牌
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    //升级效果:伤害数值增加3
}
