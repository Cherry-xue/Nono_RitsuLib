using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.Characters;
using Nono.NonoCode.Powers;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Nono.NonoCode.Cards;

// 注册成人物起始卡
[RegisterCharacterStarterCard(typeof(NonoCharacter), 1)]

// 火球术-造成伤害并附加燃烧效果
public class FireBall : NonoCard
{   
    public FireBall() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
    //定义卡牌基本属性：0能量，攻击，基础稀有度，目标为任意敌人,在图鉴中可见
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Move),
        new DynamicVar("Burn", 3m)
    ];
    //定义可变参数：伤害数值，初始值为4,Burn数值，初始值为3
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //定义卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    //定义提示：提示内容为BurnPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
        await PowerCmd.Apply<BurnPower>(choiceContext, cardPlay.Target, DynamicVars["Burn"].BaseValue, Owner.Creature, this);
    }
    //卡牌效果：对目标造成等同于DynamicVars.Damage数值的伤害
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Burn"].UpgradeValueBy(1m);
    }
    //升级效果：伤害数值增加2,Burn数值增加1
}
