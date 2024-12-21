using System.Runtime.Serialization;

namespace TradingCards.Cards;

public class LorcanaCard : CardBase
{
    public required int InkCost { get; set; }
    public required LorcanaRarity Rarity { get; set; }
}

public enum LorcanaRarity
{
    Common,
    Enchanted,
    Legendary,
    Promo,
    Rare,
    Uncommon,

    [EnumMember(Value = "Super Rare")]
    SuperRare,
}
