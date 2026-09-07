using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Cards.Common;

// 点燃-施加燃烧
public class Ignite : NonoCard
{
    public Ignite() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
    //定义卡牌基本属性：0能量，技能，普通稀有度，目标为任意敌人
    {
        this.SecondaryCosts().Set(ModResources.ManaId, 1);
    }
    //定义魔力消耗为1
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("Burn", 5m)
    ];
    //定义可变参数：灼烧数值，初始值为5
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NonoKeywords.MagicCard];
    //定义卡牌关键词：魔法牌
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    //定义提示：提示BurnPower的相关信息
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BurnPower>(choiceContext, cardPlay.Target, base.DynamicVars["Burn"].BaseValue, base.Owner.Creature, this);
    }
    //卡牌效果：使目标获得等同于DynamicVars["Burn"]数值的BurnPower
    protected override void OnUpgrade()
    {
        DynamicVars["Burn"].UpgradeValueBy(2m);
    }
    //升级效果：灼烧数值增加2
}