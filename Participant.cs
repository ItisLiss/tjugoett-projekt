class Participant
{
    public string Name { get; }
    public Hand Hand { get; }

    protected Participant(string name)
    {
        Name = name;
        Hand = new Hand();
    }
}