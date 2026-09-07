using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.Characters;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Nono.NonoCode.Cards;

// 注册成人物起始卡，后面是数量。不需要删除即可。
[RegisterCharacterStarterCard(typeof(NonoCharacter), 4)]

// 打击-对目标造成伤害
public class NonoAttack() : NonoCard
    (1,CardType.Attack, CardRarity.Basic,TargetType.AnyEnemy, true)
    //定义卡牌基本属性：1能量，攻击，基础稀有度，目标为任意敌人
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    //定义打击词条
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
    //定义可变参数：伤害数值，初始值为6

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
    }
    //卡牌效果：对目标造成等同于DynamicVars.Damage数值的伤害
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    //升级效果：伤害数值增加3
}
