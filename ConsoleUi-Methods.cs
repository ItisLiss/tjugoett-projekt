using System;
using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public partial class ConsoleUi
    {
        // --- NY HJÄLPMETOD FÖR ATT VISA HANDEN SNYGGT ---
        private void DisplayPlayerHand(Player player)
        {
            Console.WriteLine("\nDin hand:");
            PrintCardsSideBySide(player.Hand.Cards);

            // Här använder vi handens inbyggda egenskaper som vi skapade tidigare!
            Console.WriteLine($"Total poäng: {player.Hand.Value}");

            if (player.Hand.IsBlackJack)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  --> BLACKJACK!");
                Console.ResetColor();
            }
            else if (player.Hand.IsSoft)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  (Mjuk hand - ess räknas som 11 eller 1)");
                Console.ResetColor();
            }
        }

        private void PrintCardsSideBySide(IEnumerable<Card> cards)
        {
            CardRenderer renderer = new();
            List<string[]> renderedCards = new();
            List<Suit?> suits = new();

            foreach (Card card in cards)
            {
                renderedCards.Add(renderer.Render(card));
                suits.Add(card.Suit);
            }

            PrintCardRowsSideBySide(renderedCards, suits);
        }

        private void PrintCardRowsSideBySide(List<string[]> renderedCards, List<Suit?> suits)
        {
            if (renderedCards.Count == 0)
            {
                return;
            }

            for (int row = 0; row < renderedCards[0].Length; row++)
            {
                for (int cardIndex = 0; cardIndex < renderedCards.Count; cardIndex++)
                {
                    string cardRow = renderedCards[cardIndex][row];

                    if (row == 3 &&
                        (suits[cardIndex] == Suit.Hearts ||
                         suits[cardIndex] == Suit.Diamonds))
                    {
                        Console.Write(cardRow[..4]);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(cardRow[4]);
                        Console.ResetColor();
                        Console.Write(cardRow[5..]);
                    }
                    else
                    {
                        Console.Write(cardRow);
                    }

                    Console.Write(" ");
                }

                Console.WriteLine();
            }
        }

        public List<Player> AddPlayers()
        {
            int playerCount = ReadPlayerCount();
            List<Player> players = new();

            for (int i = 0; i < playerCount; i++)
            {
                string name = ReadPlayerName(i + 1);
                players.Add(new Player(name));
            }

            Console.Write("Hur många datorspelare vill du lägga till? (0 för inga): ");
            string? input = Console.ReadLine();
            // Försöker omvandla användarens svar till ett heltal.
            bool success = int.TryParse(input, out int computerCount);
            // Kontrollerar om användaren skrev ett ogiltigt tal eller negativt antal
            while (!success || computerCount < 0)
            {

                // Visar ett felmeddelande om antalet är ogiltigt
                Console.WriteLine("Ogiltigt antal datorspelare.");
                // Ber användaren skriva ett nytt antal
                Console.Write("Ange antal datorspelare igen: ");
                input = Console.ReadLine();
                // Försöker omvandla det nya svaret till ett heltal.
                success = int.TryParse(input, out computerCount);

            }
            // Upprepar koden en gång för varje datorspelare.
            for (int i = 0; i < computerCount; i++)
            {
                // Skapar en ny datorspelare med ett automatiskt namn
                Player bot = new Player($"Datorspelare {i + 1}");
                // Ger datorspelaren en enkel strategy för att välja Hit eller Stand
                if (i % 2 == 0)
                {
                    bot.Strategy = new SimplePlayerStrategy();
                }
                else
                {
                    bot.Strategy = new BasicPlayerStrategy();
                }
                // Lägger till datorspelaren i listan med alla spelare.
                players.Add(bot);
            }
            return players;
        }

        public void DisplayRules()
        {
            Console.Clear();
            Console.WriteLine("--- REGLER ---");
            Console.WriteLine("Kom så nära 21 som möjligt utan att överstiga 21.");
            Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
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

        private static string OutComeText(Outcome outcome) => outcome switch
        {
            Outcome.Win => "Vinst",
            Outcome.Lose => "Förlust",
            Outcome.Push => "Lika",
            Outcome.Blackjack => "Blackjack",
            _ => ""
        };

        private void PrintCard(Card card)
        {
            // Gör om enum-värdet till en sträng (t.ex. "Hearts" -> "Hearts")
            string suit = card.Suit.ToString();

            string suitSymbol = suit switch
            {
                "Hearts" or "Hjärter" => "♥",
                "Diamonds" or "Ruter" => "♦",
                "Spades" or "Spader" => "♠",
                "Clubs" or "Klöver" => "♣",
                _ => suit
            };

            if (suit.Equals("Hearts", StringComparison.OrdinalIgnoreCase) ||
                suit.Equals("Diamonds", StringComparison.OrdinalIgnoreCase) ||
                suit.Equals("Hjärter", StringComparison.OrdinalIgnoreCase) ||
                suit.Equals("Ruter", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine($"  [{card.Rank} {suitSymbol}]");
            Console.ResetColor();
        }

        private static void PlaceBetForPlayer(Player player)
        {
            // Säkriv att gamla insatser är nollställda inför en ny runda
            player.CurrentBet = 0;
            // Om spelaren är en datorspelare
            if (player.Strategy != null)
            {
                // Datorspelare satsar t.ex. 10% av sina marker, dock minst 10 eller allt de har kvar
                int botBet = Math.Max(10, player.Chips / 10);
                botBet = Math.Min(botBet, player.Chips);
                player.PlaceBet(botBet);
                Console.WriteLine($"{player.Name} satsar {player.CurrentBet} marker (saldo: {player.Chips} marker).");
                return;
            }

            // Mänsklig spelare
            while (true)
            {
                Console.WriteLine($"\n{player.Name}, du har {player.Chips} marker.");
                Console.Write("Hur mycket vill du satsa? ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int amount))
                {
                    if (player.PlaceBet(amount))
                    {
                        Console.WriteLine($"Insats godkänd! Du satsade {amount} marker (kvar: {player.Chips} marker).");
                        break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ogiltig insats! Du måste satsa mer än 0 och max vad du har ({player.Chips} marker).");
                Console.ResetColor();
            }
        }

    }

}
