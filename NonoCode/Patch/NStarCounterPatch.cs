using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;

[HarmonyPatch(typeof(NStarCounter))]
public static class NStarCounterPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("_Ready")]
    public static async void ReadyPostfix(NStarCounter __instance)
    {
        await ((GodotObject)__instance).ToSignal((GodotObject)(object)((Node)__instance).GetTree(), SceneTree.SignalName.ProcessFrame);
        object? obj = typeof(NStarCounter).GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(__instance);
        Player player = (Player)((obj is Player) ? obj : null);
        if (((player != null) ? player.Character : null) is Nono.NonoCode.Characters.NonoCharacter)
        {
            ReplaceHoverTip(__instance);
            ReplaceIcon(__instance);
        }
    }

    private static void ReplaceHoverTip(NStarCounter instance)
    {
        LocString val = new LocString("static_hover_tips", "MANA_COUNTER.title");
        LocString val2 = new LocString("static_hover_tips", "MANA_COUNTER.description");
        val2.Add("singleManaIcon", "[img]res://Nono/Images/Packed/Sprite_Fonts/mana_icon.png[/img]");
        val2.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
        HoverTip val3 = new HoverTip(val, val2, (Texture2D)null);
        typeof(NStarCounter).GetField("_hoverTip", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(instance, val3);
        Log.Info(">>>[NonoMod]--patch-- star counter hover tip replaced", 2);
    }
    private static void ReplaceIcon(NStarCounter instance)
    {
        var iconControl = HarmonyLib.Traverse.Create(instance).Field("_icon").GetValue<Control>();
        var RotationLayers = HarmonyLib.Traverse.Create(instance).Field("_rotationLayers").GetValue<Control>();
        Texture2D energy_mana = ResourceLoader.Load<Texture2D>("res://Nono/Images/Ui/Combat/energy_mana.png");
        Texture2D energy_mana_layer_2 = ResourceLoader.Load<Texture2D>("res://Nono/Images/Ui/Combat/energy_mana_layer_2.png");
        Texture2D energy_mana_layer_3 = ResourceLoader.Load<Texture2D>("res://Nono/Images/Ui/Combat/energy_mana_layer_3.png");
        if (iconControl is TextureRect texRect)
        {
            texRect.Texture = energy_mana;
        }
        if (RotationLayers != null)
        {
            var layer1 = RotationLayers.GetNodeOrNull<TextureRect>("Layer1");
            var layer2 = RotationLayers.GetNodeOrNull<TextureRect>("Layer2");
            if (layer1 != null) layer1.Texture = energy_mana_layer_2;
            if (layer2 != null) layer2.Texture = energy_mana_layer_3;
        }
        Log.Info(">>>[NonoMod]--patch-- star counter icon replaced", 2);
    }
}
