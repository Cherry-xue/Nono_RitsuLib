using Godot;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.SecondaryResources;

public static class ModResources
{
    public static SecondaryResourceDefinition ManaDefinition { get; private set; } = null!;
    public static string ManaId { get; private set; } = string.Empty;

    public static void Register()
    {
        var registry = RitsuLibFramework.GetSecondaryResourceRegistry(MainFile.ModId);

        // 这是一个“魔力值”资源
        ManaDefinition = registry.Register("mana", new SecondaryResourceDefinition(
            defaultAmount: 0,
            baseMaxAmount: null,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.None,
            smallIconPath: "res://Nono/Images/Packed/Sprite_Fonts/mana_icon.png",
            largeIconPath: "res://Nono/Images/Ui/Combat/energy_mana.png"
        ));
        ManaId = ManaDefinition.Id;
        registry.RegisterCombatUi(
    "mana_combat_counter",
    parent =>
    {
        var row = NSecondaryResourceCounter.Create(ManaDefinition, new SecondaryResourceCounterStyle
        {
            FontSize = 32,
            PositiveColor = Colors.Cyan,
            FormatAmount = (amount, max) => amount.ToString(),
            AmountLabelOffset = new Vector2(25f, 25f),
            IconStyle = SecondaryResourceIconStyle.Default with
            {
                Size = new Vector2(96f, 96f),
                HoverTip = SecondaryResourceHoverTipStyle.Default,
            },
        });
        // 自由指定位置。例如这里我们找到能量计数器的位置，放在它旁边
        var energyCounter = parent.GetNode<Control>("%EnergyCounterContainer");
        row.Position = energyCounter.Position + new Vector2(100f, 50f);
        return row;
    },
    ctx => ctx.Node.Bind(ctx.Player)
);

        // 卡牌面上的次级资源费用显示。使用的图标就是你注册时提供的图标
        registry.RegisterCardUi(
            "mana_card_ui",
            parent =>
            {
                var ui = NSecondaryResourceCardCostUi.Create(ManaId, new SecondaryResourceCardCostUiStyle
                {
                    IconSize = new Vector2(48, 48),
                    FontSize = 24,
                });
                // 自由指定位置。例如这里我们找到能量图标的位置，放在它旁边
                var energyIcon = parent.GetNode<TextureRect>("%EnergyIcon");
                ui.Position = energyIcon.Position + new Vector2(-10, 45);
                return ui;
            },
            ctx => ctx.Node.Refresh(ctx)
        );

        // 限定仅对特定角色始终显示
        // registry.AlwaysShowInCombatUiForCharacter<NonoCharacter>(ManaDefinition.LocalId);
        // 永远显示（不受角色限制）
        registry.AlwaysShowInCombatUi(ManaDefinition.LocalId);
    }
}