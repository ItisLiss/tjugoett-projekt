using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public class Hand
    {
        // En privat lista för att kapsla in korten så ingen utifrån kan manipulera dem direkt
        private readonly List<Card> _cards = new();

        // Publika egenskapen som exponerar korten som skrivskyddade (Read-Only)
        public IReadOnlyList<Card> Cards => _cards;

        // Egenskap för att få handens totala poäng (använder vår nya hjälpmetod)
        public int Value
        {
            get
            {
                CalculateHand(out int finalValue, out _);
                return finalValue;
            }
        }

        // Egenskap som kollar om handen är "soft" (dvs. har ett ess som räknas som 11 utan att spräcka)
        public bool IsSoft
        {
            get
            {
                CalculateHand(out _, out int remainingAces);
                return remainingAces > 0;
            }
        }

        // Returnerar true om poängen är över 21 (spelaren har spräckt)
        public bool IsBust => Value > 21;

        // Returnerar true om det är exakt 2 kort och värdet är 21 (Blackjack!)
        public bool IsBlackJack => _cards.Count == 2 && Value == 21;

        // Lägger till ett kort i handen
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        // Tömmer handen inför en ny runda
        public void Clear()
        {
            _cards.Clear();
        }

        // --- HJÄLPMETOD (DRY - Don't Repeat Yourself) ---
        // Den här metoden räknar ut både slutpoäng och hur många ess som fortfarande räknas som 11.
        // Genom att göra så här slipper vi skriva samma loop i både Value och IsSoft!
        private void CalculateHand(out int totalValue, out int acesAsEleven)
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

            // Dynamisk ess-hantering: Om vi har över 21 och har ess (som startade på 11), 
            // drar vi av 10 poäng per ess tills vi är på eller under 21.
            while (value > 21 && aces > 0)
            {
                value -= 10;
                aces--; // Ett ess till har tvingats bli en 1:a
            }

            totalValue = value;
            acesAsEleven = aces; // Om aces > 0 här, betyder det att vi har minst ett ess som fortfarande räknas som 11 (Soft hand)
        }
    }
}