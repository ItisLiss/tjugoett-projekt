using Kunskapskontroll3;

class Dealer : Participant
{
    private int StandValue = 17;
    public bool HoleCardRevealed {get; private set;} = false;

    public Dealer() : base("Givaren")
    {
    }

    public Move DecideMove()
    {
        return Hand.Value < StandValue ? Move.Hit : Move.Stand;
    }

    public void RevealHoleCard()
    {
        HoleCardRevealed = true;
    }

    public void PlayTurn(Deck deck)
    {
        RevealHoleCard();

        if(DecideMove() == Move.Hit)
        {
            ReceiveCard(deck.DealCard());
        }
    }

    public string ShowHand()
    {
        if(HoleCardRevealed || Hand.Cards.Count < 2)
            return string.Join(" ", Hand.Cards);
        else 
            return $"{Hand.Cards[0]} ??";
    }
}