using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;

namespace Nono.NonoCode.Cards;

public class EmberStrength() : NonoCard
    (0, CardType.Power, CardRarity.Token, TargetType.Self,true)
//定义卡牌基本属性：0能量，技能，Token稀有度，目标为自身
{
    public override bool CanBeGeneratedInCombat => false;
    //定义不能在战斗中生成
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("EmberStrength", 10m),
        new DynamicVar("Upgrade", 2m)
    ];
    //定义可变参数:抽取卡牌数,初始值为3
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            //升级当前模态
            //获取当前玩家的余烬之力层数
            int power_amount = Owner.Creature.GetPowerAmount<EmberStrengthPower>();
            int upgrade_amount = 0;
            //如果余烬之力层数减去升级数值小于1,则升级数值为余烬之力层数减去1,否则升级数值为DynamicVars["Upgrade"].BaseValue
            if (power_amount - DynamicVars["Upgrade"].BaseValue < 1){
                upgrade_amount = power_amount - 1;
            }
            else
            {
                upgrade_amount = (int)DynamicVars["Upgrade"].BaseValue;
            }
            if (upgrade_amount > 0) 
            { 
                await PowerCmd.Apply<EmberStrengthPower>(choiceContext, Owner.Creature, -upgrade_amount, Owner.Creature, this);
            }
        }
        else
        {
            //切换模态时,移除其他模态,并施加余烬之力
            await PowerCmd.Remove<ReignitingFlamePower>(Owner.Creature);
            await PowerCmd.Apply<EmberStrengthPower>(choiceContext, Owner.Creature, DynamicVars["EmberStrength"].BaseValue, Owner.Creature, this);
        }
    }
    //卡牌效果:抽取等同于DynamicVars.Cards数值的卡牌
}