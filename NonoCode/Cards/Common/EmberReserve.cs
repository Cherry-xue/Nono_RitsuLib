using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.Cards;

// 蓄火-获得格挡并获得预燃效果
public class EmberReserve() : NonoCard
    (1, CardType.Skill, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：1能量，技能，罕见稀有度，目标为自己
{
    public override bool GainsBlock => true;
    //卡牌属性：提供格挡
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("PreBurning", 2m),
        new BlockVar(7m, ValueProp.Move)
    ];
    //定义可变参数：PreBurning数值，初始值为2；Block数值，初始值为7
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<PreBurningPower>()];
    //定义提示：提示PreBurningPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<PreBurningPower>(choiceContext, Owner.Creature, DynamicVars["PreBurning"].BaseValue, Owner.Creature, this);
    }
    //卡牌效果:获得等同于DynamicVars.Block数值的格挡，并获得等同于DynamicVars["PreBurning"]数值的PreBurningPower
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
        DynamicVars["PreBurning"].UpgradeValueBy(1m);
    }
    //升级效果:获得的格挡数值增加5，PreBurning数值增加1
}
