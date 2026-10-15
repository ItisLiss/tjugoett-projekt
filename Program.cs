using System;

namespace Kunskapskontroll3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1. Skapa instansen av vårt användargränssnitt
                ConsoleUi ui = new();

                // 2. Starta spelets huvudloop (som hanterar menyer och val)
                ui.RunGameLoop();
            }
            catch (Exception ex)
            {
                // 3. Fånga upp oväntade krascher så programmet dör snyggt (för Trello-krysset)
                Console.Clear();
                Console.WriteLine("Ett oväntat fel uppstod: " + ex.Message);
                Console.WriteLine("Tryck på valfri tangent för att stänga av programmet.");
                Console.ReadKey();
            }
        }
    }
}