using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.Cards.Common;

// 怕痛就点防御-随机制作格挡药水
public class PumpDefenseIfScared() : NonoCard
    (1, CardType.Skill, CardRarity.Common, TargetType.Self,true)
//定义卡牌基本属性：1能量，技能，普通稀有度，目标为自身
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        NonoKeywords.PotionMaking
    ];
    //卡牌关键词:消耗,药水制作
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("PotionCount", 1m)];
    //定义可变参数：制作的药水数量，初始值为1
    private readonly List<PotionModel> PotionPool =
    [
        ModelDb.Potion<IronskinPotion>(),       //铁皮药水
        ModelDb.Potion<IronskinPotion>(),
        ModelDb.Potion<IronskinPotion>(),

        ModelDb.Potion<BlockPotion>(),          //格挡药水
        ModelDb.Potion<BlockPotion>(),
        ModelDb.Potion<BlockPotion>(),
        ModelDb.Potion<BlockPotion>(),

        ModelDb.Potion<ShipInABottle>(),        //瓶中船
        ModelDb.Potion<ShipInABottle>(),

        ModelDb.Potion<HeartOfIron>(),          //铁心药水
    ];
    //定义药水池
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars["PotionCount"].IntValue; i++)
        {
            PotionModel potionModel = PotionPool[Owner.RunState.Rng.CombatPotionGeneration.NextInt(PotionPool.Count)];
            await PotionCmd.TryToProcure(potionModel.ToMutable(), Owner, -1);
        }
    }
    //卡牌效果：随机制作PotionCount个药水
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    //升级效果:添加保留关键词
}
