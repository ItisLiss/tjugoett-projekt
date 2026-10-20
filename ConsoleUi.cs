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
            bool spelaFleraRundor = true;
            while (spelaFleraRundor)
            {
                // 0. TA BORT SPELARE SOM ÄR PANKA (0 MARKER)
                players.RemoveAll(p => p.Chips <= 0);

                if (players.Count == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Alla spelare har slut på marker! Spelet är slut.");
                    Console.ResetColor();
                    break;
                }

                deck.PrepareForNextRound();
                foreach (Player player in players)
                {
                    player.NewHand();
                }

             // --- FASE 0: INSATSER ---
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("       INSATSFASEN            ");
                Console.WriteLine("==============================");

                foreach (Player player in players)
                {
                    PlaceBetForPlayer(player);
                }

                Console.WriteLine("\nAlla insatser placerade! Tryck på valfri tangent för att dela ut korten...");
                Console.ReadKey();

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
                Console.WriteLine("Dealerns kort (det andra är dolt):");
                CardRenderer dealerCardRenderer = new();
                PrintCardRowsSideBySide(
                    new List<string[]>
                    {
                        dealerCardRenderer.Render(dealer.Hand.Cards[0]),
                        dealerCardRenderer.RenderBack()
                    },
                    new List<Suit?>
                    {
                        dealer.Hand.Cards[0].Suit,
                        null
                    });

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
                        // Visar när datorspelaren väljer att köpa ett kort.
                        if (move == Move.Hit)
                        {
                            Console.WriteLine($"{player.Name} köper ett kort.");
                        }
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
            PrintCardsSideBySide(dealer.Hand.Cards);

            // 4. FASE 3: JÄMFÖR OCH VISA ALLA RESULTAT
            Console.WriteLine("\n==============================");
            Console.WriteLine("       SLUTRESULTAT           ");
            Console.WriteLine("==============================");

            foreach (Player player in players)
            {
                // 1. Spara insatsen i en variabel INNAN någon metod nollställer den
                int betAtStake = player.CurrentBet;

                // 2. Bestäm resultat och beräkna utbetalning (RecordOutcome anropar WinBet/LoseBet internt)
                string result = round.DetermineWinner(player, dealer);

                // Om DetermineWinner INTE anropar RecordOutcome själv, gör vi det här baserat på resultatet:
                if (result.Contains("Vunnit") || result.Contains("vunnit"))
                {
                    player.RecordOutcome(Outcome.Win);
                }
                else if (result.Contains("Förlorat") || result.Contains("förlorat") || result.Contains("sprack"))
                {
                    player.RecordOutcome(Outcome.Lose);
                }
                else if (result.Contains("Lika") || result.Contains("Push"))
                {
                    player.RecordOutcome(Outcome.Push);
                }
                else if (result.Contains("Blackjack"))
                {
                    player.RecordOutcome(Outcome.Blackjack);
                }

                // 3. Snygg och enhetlig utskrift (utan dubletter)
                Console.Write($"\nSpelare: {player.Name} ({player.Hand.Value} poäng) -> ");

                if (result.Contains("Vunnit") || result.Contains("vunnit") || result.Contains("Blackjack"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(result);
                    Console.ResetColor();
                    Console.WriteLine($"  + Plus {betAtStake} marker i vinst!");
                }
                else if (result.Contains("Förlorat") || result.Contains("förlorat") || result.Contains("sprack"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result);
                    Console.ResetColor();
                    Console.WriteLine($"  - Förlorade insatsen på {betAtStake} marker.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(result);
                    Console.ResetColor();
                    Console.WriteLine($"  Insatsen på {betAtStake} marker återlämnad.");
                }

                Console.WriteLine($"  Totalt saldo: {player.Chips} marker");
            }
         
                Console.WriteLine("\n--------------------------------------------");
                Console.WriteLine("Vill ni spela en till runda? (J/N)");
                string? svar = Console.ReadLine();
                if(svar?.Trim().ToLower() != "j" && svar?.Trim().ToLower() != "ja" )
                {
                    spelaFleraRundor = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("--- Ny runda startar nu ---");
                }
            }
        Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka till menyn.");
        Console.ReadKey();
        }


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
