namespace Kunskapskontroll3
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUi ui = new ConsoleUi();
            ui.RunGameLoop(); // Startar spelet via UI-klassen
        }
    }
}