using System;
using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public class ConsoleUi
    {
        private readonly GameStorage _gameStorage = new();
        private List<Player> _savedPlayers = new();
        public void RunGameLoop()
        {
            _savedPlayers = _gameStorage.LoadPlayers();
            bool running = true;

            while (running)
            {
                DisplayMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartNewGame();
                        break;
                    case "2":
                        DisplayRules();
                        break;
                    case "3":
                        running = false;
                        DisplayMessage("Tack för att du spelade!");
                        break;
                    default:
                        DisplayMessage("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("    VÄLKOMMEN TILL TJUGOETT!   ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Starta nytt spel");
            Console.WriteLine("2. Visa regler");
            Console.WriteLine("3. Avsluta");
            Console.Write("\nVälj ett alternativ (1-3): ");
        }

        public void StartNewGame()
    {
        Console.Clear();
        Console.WriteLine("--- NYTT SPEL ---");
        List<Player> players = AddPlayers();

        Deck deck = new Deck();
        deck.Shuffle();
        Round round = new Round();

        // 1. Skapa dealern och dela ut startkort (ett synligt, ett dolt)
        Dealer dealer = new Dealer();
        dealer.ReceiveCard(deck.DealCard()); // Det synliga kortet
        dealer.ReceiveCard(deck.DealCard()); // Det dolda kortet

        // 2. FASE 1: ALLA SPELARE SPELAR KLART
        foreach (Player player in players)
        {
            Console.Clear();
            Console.WriteLine($"\n--- {player.Name}s tur ---");
            Console.WriteLine($"Dealerns synliga kort: ");
            PrintCard(dealer.Hand.Cards[0]);
            //{dealer.Hand.Cards[0].Rank} of {dealer.Hand.Cards[0].Suit}]");
            Console.WriteLine("(Det andra kortet är dolt)");

            // Dela ut två startkort
            Card card1 = deck.DealCard();
            bool busted = round.GiveCardAndCheckBust(player, card1);

            Card card2 = deck.DealCard();
            busted = round.GiveCardAndCheckBust(player, card2);

            DisplayPlayerHand(player);

            if (busted)
            {
                DisplayMessage("Du har spruckit på starthanden!");
                continue;
            }

            // Loop för spelardrag (Hit eller Stand)
            while (!busted)
            {
                // Kontrollerar om spelaren är en dator som har en strategy.
                bool isComputer = player.Strategy != null;
                // Sparar spelarens val
                string? choice;
                if (isComputer)
                    {
                        // Låter datorns strategi välja Hit eller Stand.
                        Move move = player.Strategy!.ChooseMove(player.Hand, dealer.Hand.Cards[0]);
                        // Omvandlar Hit till K och Stand till S.
                        choice = move == Move.Hit ? "K" : "S";
                        // Visar vilken drag datorn valde.
                        Console.WriteLine($"{player.Name} väljer {move}.");
                    }
                    else
                    {
                        // Låter en mänsklig spelare välja själv.
                        Console.Write("\nKöp kort (K) eller stanna (S)? ");
                        choice = Console.ReadLine();
                    }
                
                
                if (choice?.Trim().ToUpper() == "K")
                {
                    Card newCard = deck.DealCard();
                    busted = round.GiveCardAndCheckBust(player, newCard);
                    
                    DisplayPlayerHand(player);

                    if (busted)
                    {
                        DisplayMessage("Du har spruckit!");
                        break;
                    }
                }
                else if (choice?.Trim().ToUpper() == "S")
                {
                    Console.WriteLine($"{player.Name} stannar på {player.Hand.Value} poäng.");
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Välj K eller S.");
                }
            }
            
            Console.WriteLine("\nTryck på valfri tangent för att gå vidare...");
            Console.ReadKey();
        }

        // 3. FASE 2: DEALERNS TUR
        Console.Clear();
        Console.WriteLine("\n==============================");
        Console.WriteLine("       DEALERNS TUR           ");
        Console.WriteLine("==============================");

        round.PlayDealerTurn(dealer, deck);

        Console.WriteLine($"\nDealerns sluthand (Poäng: {dealer.Hand.Value}):");
        foreach (Card card in dealer.Hand.Cards)
        {
            PrintCard(card);
            //Console.WriteLine($"  - {card.Rank} of {card.Suit}");
        }

        // 4. FASE 3: JÄMFÖR OCH VISA ALLA RESULTAT
        Console.WriteLine("\n==============================");
        Console.WriteLine("       SLUTRESULTAT           ");
        Console.WriteLine("==============================");

        foreach (Player player in players)
        {
            string result = round.DetermineWinner(player, dealer);
            Console.Write($"\nSpelare: {player.Name} ({player.Hand.Value} poäng) -> ");
            
            if (result.Contains("Vunnit"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (result.Contains("Förlorat"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine(result);
            Console.ResetColor();
        }

        Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka till menyn.");
        Console.ReadKey();
    }

    // --- NY HJÄLPMETOD FÖR ATT VISA HANDEN SNYGGT ---
    private void DisplayPlayerHand(Player player)
    {
        Console.WriteLine("\nDin hand:");
        foreach (Card card in player.Hand.Cards)
        {
            //Console.WriteLine($"  - {card.Rank} of {card.Suit}");
            PrintCard(card);
        }

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
                   
                       // Visar ett felmaddelande om antalet är ogiltigt
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

    }
    
}
