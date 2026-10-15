public class Participant
{
    public string Name { get; }
    public Hand Hand { get; private set;}

    protected Participant(string name)
    {
        Name = name;
        Hand = new Hand();
    }

    public void ReceiveCard(Card card)
    {
        Hand.AddCard(card);
    }

    public virtual void NewHand()
    {
        Hand = new Hand();
    }
}
