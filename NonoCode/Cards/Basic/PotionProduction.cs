using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Nono.NonoCode.Characters;
using Nono.NonoCode.Potions;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Nono.NonoCode.Cards;

// 注册成人物起始卡，后面是数量。不需要删除即可。
[RegisterCharacterStarterCard(typeof(NonoCharacter), 1)]
public class PotionProduction() : NonoCard
    (1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
//定义卡牌基本属性：1能量，技能，基础稀有度，目标为自身
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    //卡牌关键词:消耗,药水制作
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("PotionCount", 1m)];
    //定义可变参数：制作的药水数量，初始值为1
    private readonly List<PotionModel> PotionPool =
    [
        ModelDb.Potion<LesserManaPotion>(),         //弱效魔力药水
        ModelDb.Potion<LesserSwiftPotion>(),        //弱效迅捷药水
        ModelDb.Potion<LesserHealingPotion>(),      //弱效治疗药水
        ModelDb.Potion<SwiftnessPotion>(),          //速度药水
        ModelDb.Potion<IronskinPotion>(),           //铁皮药水
        ModelDb.Potion<LesserExplosiveAmpoule>(),   //仿制爆炸安瓿
        ModelDb.Potion<LesserFirePotion>()          //仿制火焰药水
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
        DynamicVars["PotionCount"].UpgradeValueBy(1m);
        EnergyCost.UpgradeBy(-1);
    }
    //升级效果:制作的药水数量增加1
}
