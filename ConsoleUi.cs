using System;
using System.Collections.Generic;

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
            Console.WriteLine("==============================");
            Console.WriteLine("          SPELSTART           ");
            Console.WriteLine("==============================\n");

            // Skapa och blanda en ny kortlek
            Deck deck = new Deck();

            List<Card> playerCards = new List<Card>();
            List<Card> dealerCards = new List<Card>();

            // Dela ut 2 kort till spelaren
            playerCards.Add(deck.DealCard());
            playerCards.Add(deck.DealCard());

            // Dela ut 2 kort till dealern (ett synligt, ett dolt)
            dealerCards.Add(deck.DealCard()); 
            dealerCards.Add(deck.DealCard()); 

            // Skriv ut dealerns hand (första synlig, andra dold)
            Console.WriteLine("DEALERNS HAND:");
            Console.WriteLine($"  [ {dealerCards[0]} ]  [ 🂠 DOLDT KORT ]\n");

            // Skriv ut spelarens hand
            Console.WriteLine("DINA KORT:");
            foreach (var card in playerCards)
            {
                Console.WriteLine($"  [ {card} ]");
            }

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