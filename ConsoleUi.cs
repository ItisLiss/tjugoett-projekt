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
            Console.WriteLine("--- NYTT SPEL ---");
            Console.WriteLine("Spelet håller på att startas... (Här kopplas Round-klassen in sen!)");
            Console.WriteLine("\nTryck på valfri knapp för att gå tillbaka till menyn.");
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