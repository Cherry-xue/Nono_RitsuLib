using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib;
using STS2RitsuLib.Interop;

namespace Nono;

[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModId = "Nono";
    public static readonly Logger Logger = RitsuLibFramework.CreateLogger(ModId);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        // 自动注册内容
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
        // 古老牙齿可以把一张初始卡变成先古升级。
        // RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<TestCard, Shiv>();
        // 欧洛巴斯之触可以把初始遗物升级
        //RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<TestRelic, Akabeko>();
    }
}