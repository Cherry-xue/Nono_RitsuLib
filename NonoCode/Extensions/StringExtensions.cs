namespace Nono.NonoCode.Extensions;

public static class StringExtensions
{
    public static string RemovePrefix(this string id)
    {
        const string card_prefix = "nono_card_";
        if (id.StartsWith(card_prefix, StringComparison.OrdinalIgnoreCase))
            return id.Substring(card_prefix.Length);

        const string potion_prefix = "nono_potion_";
        if (id.StartsWith(potion_prefix, StringComparison.OrdinalIgnoreCase))
            return id.Substring(potion_prefix.Length);

        const string relic_prefix = "nono_relic_";
        if (id.StartsWith(relic_prefix, StringComparison.OrdinalIgnoreCase))
            return id.Substring(relic_prefix.Length);

        const string power_prefix = "nono_power_";
        if (id.StartsWith(power_prefix, StringComparison.OrdinalIgnoreCase))
            return id.Substring(power_prefix.Length);

        var index = id.IndexOf('-') + 1;
        return id.Substring(index, id.Length - index);
    }

    public static string ImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Cards", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Cards", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Powers", path);
    }
    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Powers", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Relics", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Charui", path);
    }
    public static string CharacterScenePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Scenes", path);
    }
    public static string PotionImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Potions", path);
    }
}
