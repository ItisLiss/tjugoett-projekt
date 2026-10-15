public class Player : Participant
{
    public int Wins {get; private set;}
    public int Losses {get; private set;}
    public int Pushes {get; private set;}
    public int Blackjacks {get; private set;}

    public Player(string name) : base(name)
    {
    }

    public void RecordOutCome(Outcome outcome)
    {
        switch (outcome)
        {
            case Outcome.Win:
                Wins++;
                break;
            case Outcome.Lose:
                Losses++;
                break;
            case Outcome.Push:
                Pushes++;
                break;
            case Outcome.Blackjack:
                Blackjacks++;
                break;
        }
    }

    public void SetStats(int wins, int losses, int pushes, int blackjacks)
    {
        Wins = wins;
        Losses = losses;
        Pushes = pushes;
        Blackjacks = blackjacks;
    }
}
