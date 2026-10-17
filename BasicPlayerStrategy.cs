
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
            // Strategitabell för hård 12.
            // Givarens kort: 2, 3, 4, 5, 6, 7,8, 9, 10, ess.  
            Move[] hard12Moves =
            {
                Move.Hit, Move.Hit, Move.Stand,
                Move.Stand, Move.Stand, Move.Hit,
                Move.Hit, Move.Hit, Move.Hit, Move.Hit
            };
            int index = dealerUpCard.Value - 2;
            return hard12Moves[index];
        }
        // Kontrollera om spelaren har en hård hand med 13-16 poäng.
        if (!hand.IsSoft && hand.Value >= 13 && hand.Value <= 16)
        {
            // Strategitabell för hård 13-16.
            // Givarens kort: 2, 3, 4, 5, 6, 7, 8, 9, 10, ess.
            Move[] hard13To16Moves =
            {
                Move.Stand, Move.Stand, Move.Stand,
                Move.Stand, Move.Stand, Move.Hit,
                Move.Hit, Move.Hit, Move.Hit, Move.Hit
            };
            int index = dealerUpCard.Value - 2;
            return hard13To16Moves[index];
        }
        // Köp kort om handen är mjuk och har högst 17 poäng.
        if (hand.IsSoft && hand.Value <= 17)
        {
            return Move.Hit;
        }
        // Kontrollera om spelaren har mjuk 18.
        if (hand.IsSoft && hand.Value == 18)
        {
            // köp kort om givaren visar 9, 10 eller ess.
            if (dealerUpCard.Value >= 9)
            {
                return Move.Hit;
            }
        }
        return Move.Stand;
        
    }
}