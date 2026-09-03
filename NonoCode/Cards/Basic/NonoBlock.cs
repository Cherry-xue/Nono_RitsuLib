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
public class NonoBlock() : NonoCard
    (1,CardType.Skill, CardRarity.Basic,TargetType.Self, true)
    //定义卡牌基本属性：1能量，能力，基础稀有度，目标为自己
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    //定义卡牌标签：防御
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5, ValueProp.Move)];
    //定义可变参数：Block-格挡值，初始值为5
    public override bool GainsBlock => true;
    //定义卡牌是否获得格挡：是

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }
    //卡牌效果:获得等同于DynamicVars.Block数值的格挡
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
    //升级效果:获得的格挡数值增加3
}
