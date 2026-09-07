using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Nono.NonoCode.Cards;

public class QuickNotes() : NonoCard
    (0, CardType.Skill, CardRarity.Token, TargetType.Self,true)
//定义卡牌基本属性：0能量，技能，Token稀有度，目标为自身
{
    public override bool CanBeGeneratedInCombat => false;
    //定义不能在战斗中生成
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];
    //定义可变参数:获得能量数值，初始值为1
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
    //卡牌效果:获得等同于DynamicVars.Energy数值的能量
}