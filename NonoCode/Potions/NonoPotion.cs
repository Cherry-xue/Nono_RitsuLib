using Godot;
using MegaCrit.Sts2.Core.Logging;
using Nono.NonoCode.Characters;
using Nono.NonoCode.Extensions;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Nono.NonoCode.Potions;

[RegisterPotion(typeof(NonoPotionPool), Inherit = true)]

public abstract class NonoPotions : ModPotionTemplate
{
    public override string CustomImagePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
            Log.Info(">>>[NonoMod]PotionPath=" + path, 2);
            return ResourceLoader.Exists(path) ? path : "potion.png".PotionImagePath();
        }
    }

    public override string CustomOutlinePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
            return ResourceLoader.Exists(path) ? path : "potion.png".PotionImagePath();
        }
    }
}