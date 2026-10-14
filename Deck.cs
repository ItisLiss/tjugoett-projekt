class Deck
{
    List<Card> cards = new List<Card>();

    public Deck()
    {
        foreach(Suit suit in Enum.GetValues<Suit>())
        {
            foreach(Rank rank in Enum.GetValues<Rank>())
            {
                cards.Add(new Card(suit,rank));
            }
        }
    }
}