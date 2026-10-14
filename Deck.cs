
    public class Deck
    {
        private List<Card> cards = new List<Card>();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                foreach (Rank rank in Enum.GetValues<Rank>())
                {
                    cards.Add(new Card(suit, rank));
                }
            }
            Shuffle(); // Blanda kortleken direkt när den skapas
        }

        public void Shuffle()
        {
            Random rand = new Random();

            for (int i = cards.Count - 1; i > 0; i--)
            {
                int r = rand.Next(i + 1);
                
                // Använder C# tuples för att byta plats på korten smidigt
                (cards[i], cards[r]) = (cards[r], cards[i]);
            }
        }

        // Metod för att dela ut (dra) ett kort från toppen av leken
        public Card DealCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Kortleken är slut!");
            }

            Card cardToDeal = cards[0];
            cards.RemoveAt(0); // Tar bort kortet så det inte kan dras igen
            return cardToDeal;
        }

        public int Total()
        {
            return cards.Count;
        }
    }
