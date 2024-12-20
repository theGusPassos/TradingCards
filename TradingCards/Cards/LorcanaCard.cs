namespace TradingCards.Cards;

public class LorcanaCard : CardBase
{
    public required int InkCost { get; set; }
    public required LorcanaRarity LorcanaRarity { get; set; }
}

public enum LorcanaRarity
{
    COMMON,
    ENCHANTED,
    LEGENDARY,
    PROMO,
    RARE,
    SUPER_RARE,
    UNCOMMON,
}
