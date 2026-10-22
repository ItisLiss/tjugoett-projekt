using System;
using System.Collections.Generic;
using System.IO;

namespace Kunskapskontroll3
{
    public class GameStorage
    {
        public void SavePlayers(List<Player> players)
        {
            List<string> lines = new();

            foreach (Player player in players)
            {
                string line = $"{player.Name};{player.Wins};{player.Losses};{player.Blackjacks};{player.Chips}";
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
                if(line.Trim() == "") continue;
                string[] parts = line.Split(';');

                if (parts.Length != 5)
                {
                    continue; // hoppa över rader med fel antal fält
                }

                if (string.IsNullOrWhiteSpace(parts[0]))
                {
                    continue; // hoppa över rader utan namn
                }

                if (!int.TryParse(parts[1], out int wins) ||
                    !int.TryParse(parts[2], out int losses) ||
                    !int.TryParse(parts[3], out int blackjacks) ||
                    !int.TryParse(parts[4], out int chips) ||
                    wins < 0 || losses < 0 || blackjacks < 0 || chips < 0)
                {
                    continue; // hoppa över rader med ogiltig statistik
                }

                players.Add(new Player(parts[0].Trim(), wins, losses, blackjacks, chips));
            }

            return players;
        }
    }
}
