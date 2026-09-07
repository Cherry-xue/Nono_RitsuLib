using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.Cards;

// 柴薪-获得预燃
public class FireWood() : NonoCard
    (1, CardType.Skill, CardRarity.Common, TargetType.Self,true)
    //定义卡牌基本属性：1能量，技能，普通稀有度，目标为自己
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("PreBurning", 4m)];
    //定义可变参数：PreBurning数值，初始值为4
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PreBurningPower>()
    ];
    //定义提示：提示PreBurningPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PreBurningPower>(choiceContext, Owner.Creature, DynamicVars["PreBurning"].BaseValue, Owner.Creature, this);
    }
    //卡牌效果:获得PreBurningPower效果,层数等同于DynamicVars["PreBurning"]数值.
    protected override void OnUpgrade()
    {
        DynamicVars["PreBurning"].UpgradeValueBy(2m);
    }
    //升级效果:PreBurning数值增加2
}
