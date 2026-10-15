public class Player : Participant
{
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Blackjacks { get; private set; }

    public Player(string name) : base(name)
    {
    }

    public void RecordOutcome(Outcome outcome)
    {
        switch (outcome)
        {
            case Outcome.Win:
                Wins++;
                break;

            case Outcome.Lose:
                Losses++;
                break;

            case Outcome.Blackjack:
                Wins++;
                Blackjacks++;
                break;

            case Outcome.Push:
                break;
        }
    }
}
