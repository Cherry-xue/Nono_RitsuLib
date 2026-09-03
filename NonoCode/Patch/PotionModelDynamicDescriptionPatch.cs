using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
[HarmonyPatch(typeof(PotionModel), "DynamicDescription", MethodType.Getter)]
public static class PotionModelDynamicDescriptionPatch
{
    [HarmonyPostfix]
    public static void Postfix(ref LocString __result)
    {
        __result.Add("singleManaIcon", "[img]res://Nono/Images/Packed/Sprite_Fonts/mana_icon.png[/img]");
        Log.Info(">>>[NonoMod]--patch-- potion dynamic description add mana icon", 2);
    }
}
