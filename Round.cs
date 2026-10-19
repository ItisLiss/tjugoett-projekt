using System.Collections.Generic;

namespace Kunskapskontroll3
{

class Round
{

    
    public bool GiveCardAndCheckBust(Participant participant, Card card)
    // Ge ett kort och kontrollera om spelaren har spruckit.
    {
        participant.ReceiveCard(card);
        int total = participant.Hand.Value;
       return IsBusted(total);
        }

        public bool IsBusted(int total)
        {
            return total > 21;
        }

        // Låt dealern spela sin tur automatiskt (stannar på 17)
        public void PlayDealerTurn(Dealer dealer, Deck deck)
        {
            // Dealern drar kort så länge poängen är under 17
            while (dealer.Hand.Value < 17 || (dealer.Hand.Value == 17 && dealer.Hand.IsSoft && Config.ShouldHitOnSoft))
            {
                Card card = deck.DealCard();
                dealer.ReceiveCard(card);
            }
        }

        // Jämför spelare och dealer för att avgöra vinnare
        public string DetermineWinner(Player player, Dealer dealer)
        {
            int playerVal = player.Hand.Value;
            int dealerVal = dealer.Hand.Value;

            if (player.Hand.IsBust)
            {
                return "Förlorat (Du har spruckit)";
            }
            if (dealer.Hand.IsBust)
            {
                return "Vunnit! (Dealern sprack)";
            }
            if (playerVal > dealerVal)
            {
                return "Vunnit!";
            }
            if (playerVal < dealerVal)
            {
                return "Förlorat (Dealern har högre poäng)";
            }
            return "Oavgjort!";
        }
    }
}