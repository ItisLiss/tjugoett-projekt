using System;
using System.Collections.Generic;

namespace Kunskapskontroll3
{
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
            Shuffle();
        }

        public void Shuffle()
        {
            Random rand = new Random();

            for (int i = cards.Count - 1; i > 0; i--)
            {
                int r = rand.Next(i + 1);
                (cards[i], cards[r]) = (cards[r], cards[i]);
            }
        }

        public Card DealCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Kortleken är slut!");
            }

            Card cardToDeal = cards[0];
            cards.RemoveAt(0);
            return cardToDeal;
        }

        public int Total()
        {
            return cards.Count;
        }
    
    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("Kortleken är tom.");
        }
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
    
}
}