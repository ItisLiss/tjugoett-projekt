public class Card
{
    public Suit Suit { get; set; }
    public Rank Rank { get; set; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    // Gör det enkelt att skriva ut kortet snyggt i konsolen sedan (t.ex. ♥A eller ♠10)
    public override string ToString()
    {
        string suitSymbol = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => ""
        };

        string rankSymbol = Rank switch
        {
            Rank.Ace => "A",
            Rank.King => "K",
            Rank.Queen => "Q",
            Rank.Jack => "J",
            _ => ((int)Rank).ToString()
        };

        return $"{suitSymbol}{rankSymbol}";
    }
}