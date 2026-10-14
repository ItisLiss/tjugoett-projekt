using System;
using System.Collections.Generic;

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

    public void StartNewGame()
    {
        Console.Clear();
        Console.WriteLine("--- NYTT SPEL ---");
        List<Player> players = AddPlayers();

        Console.WriteLine("\nSpelare vid bordet:");
        foreach (Player player in players)
        {
            Console.WriteLine(player.Name);
        }

        Console.WriteLine("\nSpelrundan är inte inkopplad ännu.");
        Console.WriteLine("Tryck på valfri tangent för att gå tillbaka till menyn.");
        Console.ReadKey();
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
}
