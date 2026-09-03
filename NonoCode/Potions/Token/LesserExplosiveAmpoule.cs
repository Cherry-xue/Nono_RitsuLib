using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Nono.NonoCode.Potions;

public sealed class LesserExplosiveAmpoule : NonoPotions
{
    public override PotionRarity Rarity => PotionRarity.Token;
    //药水稀有度为衍生物
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    //药水使用范围为战斗中使用
    public override TargetType TargetType => TargetType.AllEnemies;
    //药水使用目标为所有敌人
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Unpowered)];
    //药水使用效果为对所有敌人造成伤害，伤害数值等同于DynamicVars.Damage的数值，初始值为6
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        Creature player = Owner.Creature;
        DamageVar damage = DynamicVars.Damage;
        IReadOnlyList<Creature> targets = player.CombatState.HittableEnemies;
        foreach (Creature item in targets)
        {
            NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(item));
        }
        await Cmd.CustomScaledWait(0.2f, 0.3f);
        await CreatureCmd.Damage(choiceContext, targets, damage.BaseValue, damage.Props, player, null);
    }
    //药水使用时，先获取所有可攻击的敌人，然后对每个敌人播放火焰烟雾特效，等待0.2到0.3秒后，对所有敌人造成伤害，伤害数值等同于DynamicVars.Damage的数值
}