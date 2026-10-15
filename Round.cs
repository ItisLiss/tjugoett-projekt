using System.Collections.Generic;

namespace Kunskapskontroll3
{

class Round
{
    
    public bool Turn(Participant participant, Card card)
    {
        participant.ReceiveCard(card);
        int total = participant.Hand.Value;
        if (IsBusted(total))
        {
            return true;
        }
        return false;
    }
   public bool IsBusted(int total)
    {
         return total > 21;
    }
}
}