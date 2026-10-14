public class Hand
{
    private readonly List<Card> _cards = new();

    public IReadOnlyList<Card> Cards => _cards;

    public int Value
    {
        get
        {
            int value = 0;
            int aces = 0;

            foreach (Card card in _cards)
            {
                value += card.Value;

                if (card.Rank == Rank.Ace)
                {
                    aces++;
                }
            }

            while (value > 21 && aces > 0)
            {
                value -= 10;
                aces--;
            }

            return value;
        }
    }

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }
}
