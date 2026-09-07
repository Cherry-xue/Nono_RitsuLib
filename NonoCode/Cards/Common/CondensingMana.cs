using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards;

// 凝聚魔力-获得格挡和魔力
public sealed class CondensingMana() : NonoCard
    (1, CardType.Skill, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：1能量，技能，普通稀有度，目标为自己
{
    public override bool GainsBlock => true;
    //卡牌属性：提供格挡
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(7m, ValueProp.Move),
        SecondaryResourceVars.For("Mana", ModResources.ManaId, 1)
    ];
    //定义可变参数：格挡数值，初始值为7；魔力数值，初始值为1
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await SecondaryResourceCmd.Gain(Owner, ModResources.ManaId, DynamicVars["Mana"].IntValue);
    }
    //卡牌效果：获得等同于DynamicVars.Block数值的格挡，获得等同于DynamicVars["Mana"].IntValue的魔力
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
    //升级效果：格挡数值增加3
}