using System.Text.Json.Serialization;

namespace KensaRoo.Core.Models;

public class SteelProfile
{
    [JsonPropertyName("ProfileName")]
    public required string ProfileName { get; set; }
    
    [JsonPropertyName("ProfileType")]
    public required string ProfileType { get; set; }
    
    [JsonPropertyName("Standard")]
    public required string Standard { get; set; }
    
    [JsonPropertyName("MaterialGrade")]
    public required string MaterialGrade { get; set; }
    
    [JsonPropertyName("UnitWeight")]
    public double UnitWeight { get; set; }
    
    [JsonPropertyName("Height")]
    public double? Height { get; set; }
    
    [JsonPropertyName("Width")]
    public double? Width { get; set; }
    
    [JsonPropertyName("WebThickness")]
    public double? WebThickness { get; set; }
    
    [JsonPropertyName("FlangeThickness")] 
    public double? FlangeThickness { get; set; }
    
    [JsonPropertyName("InnerRadius")]
    public double? InnerRadius { get; set; }
    
    [JsonPropertyName("ToeRadius")]
    public double? ToeRadius { get; set; }
    
    [JsonPropertyName("LegSizeA")]
    public double? LegSizeA { get; set; }
    
    [JsonPropertyName("LegSizeB")]
    public double? LegSizeB { get; set; }
    
    [JsonPropertyName("Thickness")]
    public double? Thickness { get; set; }
    
    [JsonPropertyName("OuterDiameter")]
    public double? OuterDiameter { get; set; }
    
    [JsonPropertyName("WallThickness")]
    public double? WallThickness { get; set; }
    
    [JsonPropertyName("Diameter")]
    public double? Diameter { get; set; }
}