using System;
using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public class Deck
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
            // Ser till att leken blandas direkt när den skapas!
            Shuffle();
        }

        public void Shuffle()
        {
            Random rand = new Random();

            for( int i = cards.Count -1; i > 0 ; i--)
            {
                int r = rand.Next(i + 1);
                
                //uses a tuple to swap the places of two values without the need of a temp variable.
                (cards[i], cards[r]) = (cards[r],cards[i]);
            }
        }

        // --- NY METOD: Delar ut ett kort från leken ---
        public Card DealCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Kortleken är slut!");
            }

            Card cardToDeal = cards[0];
            cards.RemoveAt(0); // Tar bort kortet så det inte dras igen
            return cardToDeal;
        }

        public int Total()
        {
            return cards.Count;
        }
    }
}