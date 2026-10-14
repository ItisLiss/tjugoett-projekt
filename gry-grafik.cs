class CardRenderer
{
    public string[] Render(Card card)
    {
        return new[]
        {
            "┌───────┐",
            "│A      │",
            "│       │",
            "│   ♥   │",
            "│       │",
            "│      A│",
            "└───────┘"
        };
    }
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
}