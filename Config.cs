public static class Config
{
    // Om Dealern ska köpa på en mjuk 17
    public static bool ShouldHitOnSoft = false;

    // Hur många kortlekar man ska ha på en gång.
    public static int DeckCount = 1;

    // När en ny kortlek ska tas fram beroände på hur många kort det är kvar i den nuvarande kortleken.
    public static int MinDeckCardAmount = 15;

    //Om en naturlig blackjack vinner över en 21 byggd på 3 eller flera kort.
    public static bool NaturalBlackjackWin = true;


    private class ConfigData
    {
        public bool ShouldHitOnSoft {get; set;}
        public int DeckCount {get; set;}
        public int MinDeckCardAmount {get; set;}
        public bool NaturalBlackjackWin {get; set;}
    }
}