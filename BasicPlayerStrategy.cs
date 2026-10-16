
namespace Kunskapskontroll3;
// Klassen ska innehålla datorspelarens grundstrategi.
public class BasicPlayerStrategy : IPlayerStrategy
{
    // Datorspelaren bestämmer nästa drag
    // Utifrån spelarens hand och givarens synliga kort.
    public Move ChooseMove(Hand hand, Card dealerUpCard)
    {
        // Köp kort om handen är hård och har högst 8 poäng.
        if (!hand.IsSoft && hand.Value <= 8)
        {
            return Move.Hit;
        }
        // Köp kort om handen är hård och har 9 till 11 poäng.
        if (!hand.IsSoft && hand.Value >= 9 && hand.Value <= 11)
        {
            return Move.Hit;
        }
        // Om spelaren har en hård hand med 12 poäng.
        if (!hand.IsSoft && hand.Value == 12)
        {
            // Stanna om givaren visar 4, 5 eller 6.
            if (dealerUpCard.Value >= 4 && dealerUpCard.Value <= 6)
            {
                return Move.Stand;
            }
             // Annars köper spelaren ett kort.
             return Move.Hit;
        }
        // Kontrollera om spelaren har en hård hand med 13-16 poäng.
        if (!hand.IsSoft && hand.Value >= 13 && hand.Value <= 16)
        {
            // Stanna om givaren visar 2 till 6.
            if (dealerUpCard.Value >= 2 && dealerUpCard.Value <= 6)
            {
                return Move.Stand;
            }
            // Annas köper spelaren ett kort.
            return Move.Hit;
        }
        return Move.Stand;
        
    }
}