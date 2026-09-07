using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace Nono.NonoCode.Singleton;

[RegisterSingleton]

//打出卷轴牌后将其从战斗中移除
public class ScrollKeywords : HookedSingletonModel
{
    public ScrollKeywords() : base(HookType.Combat)
    {
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;

        if (card.Keywords.Contains(NonoKeywords.ScrollKeywords))
        {
            await CardPileCmd.RemoveFromCombat(card, skipVisuals: false);
        }
    }
}
