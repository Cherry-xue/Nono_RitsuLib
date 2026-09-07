using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.Cards.Common;

// 来瓶基础款-随机制作普通药水
public class MakeCommonPotion() : NonoCard
    (1,CardType.Skill, CardRarity.Common,TargetType.Self,true)
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
        ModelDb.Potion<SwiftPotion>(),          //迅捷药水
        ModelDb.Potion<ExplosiveAmpoule>(),     //爆炸安瓿
        ModelDb.Potion<FirePotion>(),           //火焰药水
        ModelDb.Potion<BlockPotion>(),          //格挡药水
        ModelDb.Potion<SpeedPotion>(),          //速度药水
        ModelDb.Potion<WeakPotion>(),           //虚弱药水
        ModelDb.Potion<VulnerablePotion>(),     //易伤药水
        ModelDb.Potion<FlexPotion>(),           //肌肉药水
        ModelDb.Potion<DexterityPotion>(),      //敏捷药水
        ModelDb.Potion<StrengthPotion>(),       //力量药水
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
        EnergyCost.UpgradeBy(-1);
    }
    //升级效果:能量消耗减少1
}
