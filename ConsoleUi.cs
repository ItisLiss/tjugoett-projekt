using System;

namespace Kunskapskontroll3
{
    public class ConsoleUi
    {
        public void RunGameLoop()
        {
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
            Console.WriteLine("   VÄLKOMMEN TILL TJUGOETT!   ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Starta nytt spel");
            Console.WriteLine("2. Visa regler");
            Console.WriteLine("3. Avsluta");
            Console.Write("\nVälj ett alternativ (1-3): ");
        }

        // Tillagd metod så att kompileringsfelet försvinner!
        public void StartNewGame()
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("          SPELSTART           ");
            Console.WriteLine("==============================\n");

            // 1. Skapa och blanda kortleken
            Deck deck = new Deck();
            deck.Shuffle(); // (Om din Deck-klass har en Shuffle-metod)

            // 2. Skapa listor för spelarens och dealerns kort
            List<Card> playerCards = new List<Card>();
            List<Card> dealerCards = new List<Card>();

            // 3. Dela ut 2 kort till spelaren
            playerCards.Add(deck.DealCard());
            playerCards.Add(deck.DealCard());

            // 4. Dela ut 2 kort till dealern
            dealerCards.Add(deck.DealCard()); // Det första kortet (visas)
            dealerCards.Add(deck.DealCard()); // Det andra kortet (dolt hålkort)

            // 5. Skriva ut bordet enligt User Story 3
            Console.WriteLine("DEALERNS HAND:");
            Console.WriteLine($"  [ {dealerCards[0]} ]  [ 🂠 DOLDT KORT ]\n");

            Console.WriteLine("DINA KORT:");
            foreach (var card in playerCards)
            {
                Console.WriteLine($"  [ {card} ]");
            }

            // Räkna ut spelarens poäng med hjälp av Card-klassens .Value
            int playerTotal = playerCards[0].Value + playerCards[1].Value;
            Console.WriteLine($"\nTotalt värde på dina kort: {playerTotal}");

            Console.WriteLine("\n------------------------------");
            Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till menyn...");
            Console.ReadKey();
        }

        public void DisplayRules()
        {
            Console.Clear();
            Console.WriteLine("--- REGLER ---");
            Console.WriteLine("Kom så nära 21 som möjligt utan att bli tjock.");
            Console.WriteLine("\nTryck på valfri knapp för att gå tillbaka.");
            Console.ReadKey();
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.WriteLine("Tryck på valfri knapp för att fortsätta...");
            Console.ReadKey();
        }
    }
}