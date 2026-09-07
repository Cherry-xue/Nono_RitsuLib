using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Nono.NonoCode.Powers;
using MegaCrit.Sts2.Core.Rooms;
using Nono.NonoCode.Characters;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Nono.NonoCode.Relics;

[RegisterCharacterStarterRelic(typeof(NonoCharacter))]
public class NonoNoBag : NonoRelics
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    //设置该遗物为Starter类型，使其在角色选择界面默认装备。
    private const string _potionSlotsKey = "PotionSlots";
    //定义一个常量字符串，作为DynamicVar的键，用于表示玩家的药水槽数量。
    public override bool HasUponPickupEffect => true;
    //表示该遗物在获得时会触发特定效果。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("PotionSlots", 3m),//定义一个DynamicVar，表示玩家的药水槽数量，初始值为3。
        SecondaryResourceVars.For("Mana", ModResources.ManaId, 1)//定义一个DynamicVar，表示玩家的魔力数量，初始值为1。
    ];
    public override async Task AfterObtained()
    {
        await PlayerCmd.GainMaxPotionCount(DynamicVars["PotionSlots"].IntValue, Owner);
    }
    //当玩家获得该遗物时，调用PlayerCmd.GainMaxPotionCount命令，增加玩家的最大药水槽数量，数量等同于DynamicVars["PotionSlots"]的整数值。
    public override async Task AfterEnergyResetLate(Player player)
    {
        if (player == Owner)
        {
            await SecondaryResourceCmd.Gain(Owner, ModResources.ManaId, DynamicVars["Mana"].IntValue);
        }
    }
    //在每回合开始时，如果玩家是该遗物的拥有者,则调用SecondaryResourceCmd.Gain命令，增加玩家的魔力数量，数量等同于DynamicVars["Mana"] 的整数值。
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            await PowerCmd.Apply<EmberStrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 10, Owner.Creature, null);
        }
    }
    //当玩家进入战斗房间时，触发遗物的闪光效果，并调用PowerCmd.Apply命令，给玩家施加10层EmberStrengthPower。
}
