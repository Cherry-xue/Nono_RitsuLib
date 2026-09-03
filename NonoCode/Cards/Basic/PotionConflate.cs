using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Nono.NonoCode.Characters;
using Nono.NonoCode.PotionConflateSystem;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Nono.NonoCode.Cards;

// 注册成人物起始卡，后面是数量。不需要删除即可。
[RegisterCharacterStarterCard(typeof(NonoCharacter), 1)]
public class PotionConflate() : NonoCard
    (1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("ConflateCount", 1m),
        new BlockVar(4, ValueProp.Move)

    ];
    //定义可变参数：ConflateCount-合成药水次数，初始值为1
    public override List<CardKeyword> CanonicalKeywords => [
    ];
    //卡牌关键词:药水合成
    public override bool GainsBlock => true;
    //卡牌效果:获得格挡
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        for (int i = 0; i < DynamicVars["ConflateCount"].IntValue; i++)
        {
            await PotionConflateService.TryCraft(recipe: PotionConflateService.FindFirstCraftableRecipe(Owner.PotionSlots), owner: Owner, potionSlots: Owner.PotionSlots);
        }
    }
    //尝试进行ConflateCount次药水合成
    protected override void OnUpgrade()
    {
        DynamicVars["ConflateCount"].UpgradeValueBy(1m);
        EnergyCost.UpgradeBy(-1);
    }
    //升级效果:合成药水次数增加1
}