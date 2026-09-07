using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Nono.NonoCode;

[RegisterOwnedCardKeyword(nameof(MagicCard), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(PotionMaking), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(PotionConflation), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(VolcanoKeywords))]
[RegisterOwnedCardKeyword(nameof(ScrollKeywords), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(MagicAmplification), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Choice), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)] 
public class NonoKeywords
{
    //魔法牌
    public static readonly CardKeyword MagicCard = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(MagicCard)).GetModCardKeyword();
    //药水制作
    public static readonly CardKeyword PotionMaking = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(PotionMaking)).GetModCardKeyword();
    //药水合成
    public static readonly CardKeyword PotionConflation = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(PotionConflation)).GetModCardKeyword();
    //火山
    public static readonly CardKeyword VolcanoKeywords = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(VolcanoKeywords)).GetModCardKeyword();
    //卷轴
    public static readonly CardKeyword ScrollKeywords = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(ScrollKeywords)).GetModCardKeyword();
    //魔力增幅
    public static readonly CardKeyword MagicAmplification = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(MagicAmplification)).GetModCardKeyword();
    //抉择
    public static readonly CardKeyword Choice = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(Choice)).GetModCardKeyword();
}