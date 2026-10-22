using System;
using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public partial class ConsoleUi
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
                        Settings();
                        break;
                    case "4":
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
            ClearScreen();
            Console.WriteLine("==============================");
            Console.WriteLine("    VÄLKOMMEN TILL TJUGOETT!   ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Starta nytt spel");
            Console.WriteLine("2. Visa regler");
            Console.WriteLine("3. Visa inställningar");
            Console.WriteLine("4. Avsluta");
            Console.Write("\nVälj ett alternativ (1-4): ");
        }

        public void StartNewGame()
        {
            ClearScreen();
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
                    ClearScreen();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Alla spelare har slut på marker! Spelet är slut.");
                    Console.ResetColor();
                    break;
                }

                foreach (Player player in players)
                {
                    deck.PrepareForNextRound();
                    player.NewHand();
                }

             // --- FASE 0: INSATSER ---
                ClearScreen();
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
                    // Dela ut två startkort
                    deck.PrepareForNextRound();
                    Card card1 = deck.DealCard();
                    bool busted = round.GiveCardAndCheckBust(player, card1);

                Card card2 = deck.DealCard();
                busted = round.GiveCardAndCheckBust(player, card2);

                DrawPlayerTurn(player, dealer);

                if (busted)
                {
                    DisplayMessage("Du har spruckit på starthanden!");
                    continue;
                }
                // NY KONTROLL: Om spelaren har Blackjack på starthanden stannar turen omedelbart!
                if (player.Hand.IsBlackJack)
                {
                    Console.WriteLine($"{player.Name} har Blackjack och stannar automatiskt!");
                    continue; // Hoppar över köp-loopen och går till nästa spelare
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

                        DrawPlayerTurn(player, dealer);

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
            ClearScreen();
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
                else if (result.Contains("Lika") || result.Contains("Push") || result.Contains("Oavgjort"))
                {
                    player.RecordOutcome(Outcome.Push);
                }
                else if (result.Contains("Blackjack"))
                {
                    player.RecordOutcome(Outcome.Blackjack);
                }

                // 3. Snygg och enhetlig utskrift (utan dubletter)
                Console.Write($"\nSpelare: {player.Name} ({player.Hand.Value} poäng) -> ");
                if (result.Contains("Blackjack"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(result);
                    Console.ResetColor();
                    // Beräkna gärna 3:2-vinsten om den visas, eller skriv ut texten:
                    Console.WriteLine($"  + Blackjack! Vinner 3:2 ({Math.Round(betAtStake * 1.5)} marker i vinst)!");
                }
               
                else if (result.Contains("Vunnit") || result.Contains("vunnit"))
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

                // Spara statistik och marker för alla sparade spelare efter varje runda
                _gameStorage.SavePlayers(_savedPlayers);

                Console.WriteLine("\n--------------------------------------------");
                Console.WriteLine("Vill ni spela en till runda? (J/N)");
                string? svar = Console.ReadLine();
                if(svar?.Trim().ToLower() != "j" && svar?.Trim().ToLower() != "ja" )
                {
                    spelaFleraRundor = false;
                }
                else
                {
                    ClearScreen();
                    Console.WriteLine("--- Ny runda startar nu ---");
                }
            }
        Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka till menyn.");
        Console.ReadKey();
        }

        public void Settings()
        {
            while (true)
            {
                ClearScreen();
                Console.WriteLine("--- INSTÄLLNINGAR ---");
                Console.WriteLine(Config.PrintConfig());

                Console.WriteLine("\n1. Ändra om givaren köper på mjuk 17");
                Console.WriteLine("2. Ändra antal kortlekar");
                Console.WriteLine("3. Ändra när en ny kortlek tas fram");
                Console.WriteLine("4. Ändra om naturlig tjugoett vinner över 21 med tre eller fler kort");
                Console.WriteLine("5. Spara inställningar till fil");
                Console.WriteLine("6. Ladda inställningar från fil");
                Console.WriteLine("7. Tillbaka till menyn");
                Console.Write("\nVälj ett alternativ (1-7): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Config.ShouldHitOnSoft = ReadYesNo("Ska givaren köpa på mjuk 17?");
                        break;
                    case "2":
                        Config.DeckCount = ReadNumber("Antal kortlekar", 1, 8);
                        break;
                    case "3":
                        Config.MinDeckCardAmount = ReadNumber("Ny kortlek när färre än så här många kort är kvar", 10, 52);
                        break;
                    case "4":
                        Config.NaturalBlackjackWin = ReadYesNo("Ska naturlig tjugoett vinna över 21 med tre eller fler kort?");
                        break;
                    case "5":
                        Config.SaveConfig();
                        DisplayMessage("Inställningarna har sparats.");
                        break;
                    case "6":
                        Config.LoadConfig();
                        DisplayMessage("Inställningarna har laddats från filen.");
                        break;
                    case "7":
                        return;
                    default:
                        DisplayMessage("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        private static bool ReadYesNo(string question)
        {
            while (true)
            {
                Console.Write($"{question} \n(J/N): ");
                string? input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "J")
                {
                    return true;
                }
                if (input == "N")
                {
                    return false;
                }

                Console.WriteLine("Svara med J eller N.");
            }
        }

        private static int ReadNumber(string question, int min, int max)
        {
            while (true)
            {
                Console.Write($"{question} \n({min}-{max}): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int number) && number >= min && number <= max)
                {
                    return number;
                }

                Console.WriteLine($"Ange ett heltal mellan {min} och {max}.");
            }
        }
    }

}