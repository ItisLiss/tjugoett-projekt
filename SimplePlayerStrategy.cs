
namespace Kunskapskontroll3;

// Klassen ska innehålla datorspelarens enkla strategi
public class SimplePlayerStrategy : IPlayerStrategy

{
    //Här bestämmer om datorspelaren ska köpa kort eller stanna.
    public Move ChooseMove(Hand hand, Card dealerUpcard)
    
    {
        // Datorspelaren köper kort om poängen är under 17.
        if (hand.Value < 17)
        {
            return Move.Hit;
        }
         // datorspelaren stannar om poängen är 17 eller mer.
        return Move.Stand;
        
    }
}
