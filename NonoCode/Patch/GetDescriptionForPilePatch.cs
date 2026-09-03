using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

[HarmonyPatch]
static class GetDescriptionForPilePatch
{
    // 更稳健的定位：在 CardModel 中寻找名为 GetDescriptionForPile 的私有方法，
    // 要求参数数量为 3，且第二个参数的类型名为 "DescriptionPreviewType"（匹配嵌套私有枚举）。
    static MethodBase TargetMethod()
    {
        var cardModelType = typeof(CardModel);
        var methods = cardModelType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var m in methods)
        {
            if (m.Name != "GetDescriptionForPile")
                continue;
            var pars = m.GetParameters();
            if (pars.Length != 3)
                continue;
            if (pars[1].ParameterType.Name == "DescriptionPreviewType")
                return m;
        }
        throw new InvalidOperationException("Cannot find CardModel.GetDescriptionForPile with DescriptionPreviewType parameter.");
    }

    // Transpiler：在原来添加 "singleStarIcon" 之后，插入对 description.Add("singleManaIcon", "...") 的调用
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = instructions.ToList();

        const string manaKey = "singleManaIcon";
        const string manaImg = "[img]res://Nono/Images/Packed/Sprite_Fonts/mana_icon.png[/img]";

        // 修复：第二个参数应为 string，而不是 object
        var addMethod = AccessTools.Method(typeof(LocString), "Add", new Type[] { typeof(string), typeof(string) });
        if (addMethod == null)
            throw new InvalidOperationException("Cannot find LocString.Add(string, string) method.");

        for (int i = 0; i < codes.Count; i++)
        {
            // 找到加载常量 "singleStarIcon" 的位置
            if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand is string s && s == "singleStarIcon")
            {
                // 预计序列： ... ldloc description, ldstr "singleStarIcon", ldstr "<star img>", callvirt LocString::Add
                // 我们取前一个指令作为重新加载 description 的指令（通常是 ldloc.*）
                int loadDescIndex = i - 1;
                // 找到 callvirt 索引（通常在 i + 2）
                int callIndex = i + 2;
                if (loadDescIndex >= 0 && callIndex < codes.Count && (codes[callIndex].opcode == OpCodes.Callvirt || codes[callIndex].opcode == OpCodes.Call))
                {
                    // 把 reload description、ldstr manaKey、ldstr manaImg、callvirt Add 插入到 callvirt 之后
                    var loadDescInstr = codes[loadDescIndex];
                    var insert = new List<CodeInstruction>
                    {
                        new CodeInstruction(loadDescInstr.opcode, loadDescInstr.operand), // 再次载入 description
                        new CodeInstruction(OpCodes.Ldstr, manaKey),
                        new CodeInstruction(OpCodes.Ldstr, manaImg),
                        new CodeInstruction(OpCodes.Callvirt, addMethod)
                    };
                    codes.InsertRange(callIndex + 1, insert);
                    Log.Info(">>>[NonoMod]--patch-- card dynamic description add mana icon ", 2);
                    i += insert.Count; // 跳过新插入的指令
                }
            }
        }

        return codes.AsEnumerable();
    }
}
