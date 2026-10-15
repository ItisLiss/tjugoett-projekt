

class Round
{

    
    public bool GiveCardAndCheckBust(Participant participant, Card card)
    // Ge ett kort och kontrollera om spelaren har spruckit.
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