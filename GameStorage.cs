namespace Kunskapskontroll3
{
public class GameStorage
{
    public void SavePlayers(List<Player> players)
    {
        List<string> lines = new();

        foreach (Player player in players)
        {
            string line = $"{player.Name};{player.Wins};{player.Losses};{player.Blackjacks}";
            lines.Add(line);
        }

        File.WriteAllLines("players.txt", lines);
    }

    public List<Player> LoadPlayers()
    {
        List<Player> players = new();

        if (!File.Exists("players.txt"))
        {
            return players;
        }

        string[] lines = File.ReadAllLines("players.txt");
        foreach (string line in lines)
        {
            string[] parts = line.Split(';');

            if (parts.Length != 4)
            {
                throw new InvalidDataException("En spelarrad måste innehålla exakt fyra fält.");
            }

            if (string.IsNullOrWhiteSpace(parts[0]))
            {
                throw new InvalidDataException("En spelarrad saknar spelarnamn.");
            }

            if (!int.TryParse(parts[1], out int wins) ||
                !int.TryParse(parts[2], out int losses) ||
                !int.TryParse(parts[3], out int blackjacks) ||
                wins < 0 || losses < 0 || blackjacks < 0)
            {
                throw new InvalidDataException("En spelarrad innehåller ogiltig statistik.");
            }

            players.Add(new Player(parts[0].Trim(), wins, losses, blackjacks));
        }
        return players;
    }
}
}