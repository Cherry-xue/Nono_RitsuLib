using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Nono.NonoCode.Cards;

// 猫咬-重复多次对随机敌人施加中毒效果
public class CatBite() : NonoCard
    (2,CardType.Attack, CardRarity.Common,TargetType.RandomEnemy,true)
//定义卡牌基本属性：2能量，攻击，普通稀有度，目标为任意敌人
{
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    //卡牌关键词：保留
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<PoisonPower>(1m),
        new RepeatVar(7)
    ];
    //定义可变参数:施加的中毒数值，初始值为1;重复次数，初始值为7
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];
    //
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            Creature enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (enemy == null)
            {
                continue;
            }
            await PowerCmd.Apply<PoisonPower>(choiceContext, enemy, DynamicVars.Poison.BaseValue, Owner.Creature, this);
        }
    }
    //卡牌效果:对随机敌人施加等同于DynamicVars.Poison数值的PoisonPower，重复等同于DynamicVars.Repeat数值的次数
    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(3m);
    }
    //升级效果：中毒次数增加3
}
