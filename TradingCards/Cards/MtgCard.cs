using System.Runtime.Serialization;

namespace TradingCards.Cards;

public class MtgCard : CardBase
{
    public MtgColor? Color { get; set; }
    public required MtgRarity Rarity { get; set; }
}

public enum MtgColor
{
    [EnumMember(Value = "U")]
    BLUE,

    [EnumMember(Value = "B")]
    BLACK,

    [EnumMember(Value = "G")]
    GREEN,

    [EnumMember(Value = "R")]
    RED,

    [EnumMember(Value = "W")]
    WHITE,
}

public enum MtgRarity
{
    Common,
    Mythic,
    Rare,
    Special,
    Uncommon,
}
