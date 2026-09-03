using Godot;
using Nono.NonoCode.Extensions;
using STS2RitsuLib.Scaffolding.Content;

namespace Nono.NonoCode.Characters;

public class NonoPotionPool : TypeListPotionPoolModel
{
    public override string EnergyColorName => NonoCharacter.CharacterId;
    public override Color LabOutlineColor => NonoCharacter.Color;

    public override string BigEnergyIconPath => "Charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "Charui/text_energy.png".ImagePath();
}
