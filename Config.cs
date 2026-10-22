using System.Text.Json;
using Kunskapskontroll3;

public static class Config
{
    private const string FilePath = "config.json";
    // Om Dealern ska köpa på en mjuk 17
    public static bool ShouldHitOnSoft = false;

    // Hur många kortlekar man ska ha på en gång.
    public static int DeckCount = 1;

    // När en ny kortlek ska tas fram beroände på hur många kort det är kvar i den nuvarande kortleken.
    public static int MinDeckCardAmount = 15;

    //Om en naturlig blackjack vinner över en 21 byggd på 3 eller flera kort.
    public static bool NaturalBlackjackWin = true;

    // Körs automatiskt första gången Config används
    static Config()
    {
        Console.Clear();
        Console.WriteLine("Filen \"Config.json\" hittades inte.\nSkapat en ny med standardvärden");
        Console.WriteLine("Tryck på valfri tangent för att gå vidare...");
        Console.ReadKey();
        LoadConfig();
    }

    private class ConfigData
    {
        public bool ShouldHitOnSoft {get; set;}
        public int DeckCount {get; set;}
        public int MinDeckCardAmount {get; set;}
        public bool NaturalBlackjackWin {get; set;}
    }

    public static string PrintConfig()
    {
        return $"Inställningar: \n" +
               $"Givaren köper på mjuk 17: {(ShouldHitOnSoft ? "Ja" : "Nej")}\n" +
               $"Antal kortlekar: {DeckCount}\n" +
               $"Ny kortlek när färre än {MinDeckCardAmount} kort är kvar\n" +
               $"Naturlig tjugoett vinner över 21 med tre eller fler kort: {(NaturalBlackjackWin ? "Ja" : "Nej")}";
    }

    public static void SaveConfig()
    {
        ConfigData data = new ConfigData
        {
            ShouldHitOnSoft = ShouldHitOnSoft,
            DeckCount = DeckCount,
            MinDeckCardAmount = MinDeckCardAmount,
            NaturalBlackjackWin = NaturalBlackjackWin
        };

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static void LoadConfig()
    {
        // Ingen fil än, så skapa en med standardvärdena
        if (!File.Exists(FilePath))
        {
            SaveConfig();
            return;
        }

        try
        {
            string json = File.ReadAllText(FilePath);
            ConfigData? data = JsonSerializer.Deserialize<ConfigData>(json);

            if (data == null)
            {
                return;
            }

            ShouldHitOnSoft = data.ShouldHitOnSoft;
            DeckCount = data.DeckCount;
            MinDeckCardAmount = data.MinDeckCardAmount;
            NaturalBlackjackWin = data.NaturalBlackjackWin;
        }
        catch (JsonException)
        {
            Console.WriteLine("\"Config.cs\" filen kunde inte ladda in. \nAnvänder standard inställningar");
            // Filen är trasig, så behåll standardvärdena
        }
    }
}