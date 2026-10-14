class ConsoleUi
{
    public List<Player> AddPlayers()
    {
        int playerCount = ReadPlayerCount();
        List<Player> players = new();

        for (int i = 0; i < playerCount; i++)
        {
            string name = ReadPlayerName(i + 1);
            players.Add(new Player(name));
        }

        return players;
    }

    private static int ReadPlayerCount()
    {
        while (true)
        {
            Console.Write("Hur många ska spela? Ange 1-5: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int count) && count is >= 1 and <= 5)
            {
                return count;
            }

            Console.WriteLine("Ange ett heltal mellan 1 och 5.");
        }
    }

    private static string ReadPlayerName(int playerNumber)
    {
        while (true)
        {
            Console.Write($"Ange namn för spelare {playerNumber}: ");
            string? name = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.Trim();
            }

            Console.WriteLine("Namnet får inte vara tomt.");
        }
    }
}