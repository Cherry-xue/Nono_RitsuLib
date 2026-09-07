using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Nono.NonoCode.Cards.Common;


public class EmergencyTreatment() : NonoCard
    (0,CardType.Skill, CardRarity.Common,TargetType.Self,true)
    //定义卡牌基本属性：0能量，技能，普通稀有度，目标为自己
{
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    //卡牌关键词：消耗
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(4)];
    //定义可变参数：回复数值，初始值为4
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature,DynamicVars.Heal.BaseValue, true);
    }
    //卡牌效果：回复玩家生命，回复数值等同于DynamicVars.Heal的数值
    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2m);
    }
    //升级效果：回复数值增加2
}
