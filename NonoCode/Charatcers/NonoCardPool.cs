using Godot;
using Nono.NonoCode.Extensions;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Nono.NonoCode.Characters;

public class NonoCardPool : TypeListCardPoolModel
{
    public override string Title => NonoCharacter.CharacterId;
    public override string EnergyColorName => NonoCharacter.CharacterId;
    public override string BigEnergyIconPath => "Charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "Charui/text_energy.png".ImagePath();
    public override Color DeckEntryCardColor => NonoCharacter.Color;
    public override Color EnergyOutlineColor => new("#7D7D7D");

    private static readonly Material _poolFrameMaterial = MaterialUtils.CreateReplaceHueShaderMaterial(0.0f, 1.0f, 0.917647f); 
    public override Material PoolFrameMaterial => _poolFrameMaterial;


    public override bool IsColorless => false;

}
