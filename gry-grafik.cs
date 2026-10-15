using System.Collections.Generic;

namespace Kunskapskontroll3
{
class CardRenderer
{
    public string[] Render(Card card)
    {
        string rankSymbol = card.Rank switch
        {
            Rank.Ace => "A",
            Rank.King => "K",
            Rank.Queen => "Q",
            Rank.Jack => "J",
            _ => ((int)card.Rank).ToString()
        };

        string suitSymbol = card.Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };

        return new[]
        {
            "┌───────┐",
            $"│{rankSymbol.PadRight(7)}│",
            "│       │",
            $"│   {suitSymbol}   │",
            "│       │",
            $"│{rankSymbol.PadLeft(7)}│",
            "└───────┘"
        };
    }
}
}