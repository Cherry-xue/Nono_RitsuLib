using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Cards;
using Nono.NonoCode.Cards;
[HarmonyPatch]
public static class UpdateStarCostVisualsPatch
{
    // 精确拦截目标方法
    [HarmonyPatch(typeof(NCard), "UpdateStarCostVisuals")]
    [HarmonyPriority(int.MaxValue)]
    public static class UpdateStarCostVisuals
    {
        // 在方法执行前修改参数或执行操作
        static void Prefix(NCard __instance)
        {
            var starIcon = Traverse.Create(__instance).Field("_starIcon").GetValue<TextureRect>();
            if (__instance.Model is NonoCard mycard) { 
                Log.Info(">>>[NonoMod]-patch-UpdateStarCostVisuals Successful");
                Texture2D texture = ResourceLoader.Load<Texture2D>("res://Nono/Images/Packed/Sprite_Fonts/mana_cost_icon.png");
                starIcon.Texture = texture;
            }
            else
            {
                Log.Info(">>>[NonoMod]-patch-UpdateStarCostVisuals Skipped for non-NonoCard");
                Texture2D texture = ResourceLoader.Load<Texture2D>("res://images/ui/combat/energy_star.png");
                starIcon.Texture = texture;
            }
        }
    }
}