namespace TradingCards.Cards;

public class MtgCard : CardBase
{
    public required MtgColor Color { get; set; }
    public required MtgRarity Rarity { get; set; }
}

public enum MtgColor
{
    BLUE = 'U',
    BLACK = 'B',
    GREEN = 'G',
    RED = 'R',
    WHITE = 'W',
}

public enum MtgRarity
{
    COMMON,
    MYTHIC,
    RARE,
    SPECIAL,
    UNCOMMON,
}
