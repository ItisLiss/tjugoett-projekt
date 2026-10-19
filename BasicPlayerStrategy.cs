
namespace Kunskapskontroll3;
// Klassen ska innehålla datorspelarens grundstrategi.
public class BasicPlayerStrategy : IPlayerStrategy
{
    // Datorspelaren bestämmer nästa drag
    // Utifrån spelarens hand och givarens synliga kort.
    public Move ChooseMove(Hand hand, Card dealerUpCard)
    {
        
        // Köp kort om handen är hård och har högst 11 poäng.
        if (!hand.IsSoft && hand.Value <= 11)
        { 
            // Datorspelare väljer att köpa ett kort
            return Move.Hit;
        }
        // Om spelaren har en hård hand med 12 poäng.
        if (!hand.IsSoft && hand.Value == 12)
        {
            // Strategitabell för hård 12.
            // Givarens kort: 2, 3, 4, 5, 6, 7 ,8, 9, 10, ess.  
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
            // Strategitabell för mjukt 18 när givaren visar 2 till ess.
            // De första sju värdena betyder Stand, de sista tre betyder Hit.
            Move[] soft18Moves =
            {
                Move.Stand, Move.Stand, Move.Stand, Move.Stand, Move.Stand,
                Move.Stand, Move.Stand, Move.Hit, Move.Hit, Move.Hit
            };
            // Räknar ut vilken plats i tabellen som motsvarar givarens kort.
            int index = dealerUpCard.Value - 2;
            // Hämtar rätt beslut Hit eller Stand från strategitabellen
            return soft18Moves[index];    
            
        }
        return Move.Stand;
        
    }
}