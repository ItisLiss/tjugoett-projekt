using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public class Player : Participant
    {

        public int Wins {get; private set;}
        public int Losses {get; private set;}
        public int Pushes {get; private set;}
        public int Blackjacks {get; private set;}
        public IPlayerStrategy? Strategy { get; set; }

        // --- NYTT FÖR MARKER OCH EKONOMI ---
        public int Chips { get; set; } = 500; // Standardstartvärde om inget annat anges
        public int CurrentBet { get; set; } = 0;

        public Player(string name) : base(name)
        {
        }

        // Uppdaterad konstruktor för att kunna läsa in sparad statistik inkl. marker
        public Player(string name, int wins, int losses, int pushes, int blackjacks, int chips) : base(name)
        {
            Wins = wins;
            Losses = losses;
            Pushes = pushes;
            Blackjacks = blackjacks;
            Chips = chips;
        }

        // Konstruktör för GameStorage som matchar (namn, wins, losses, blackjacks, chips)
        public Player(string name, int wins, int losses, int blackjacks, int chips) : base(name)
        {
            Wins = wins;
            Losses = losses;
            Blackjacks = blackjacks;
            Chips = chips;
        }
        
        // Bakåtkompatibel konstruktor om sparfilen inte sparade chips tidigare
        public Player(string name, int wins, int losses, int blackjacks) : base(name)
        {
            Wins = wins;
            Losses = losses;
            Blackjacks = blackjacks;
            Chips = 500;
        }
        // --- EKONOMIMETODER ---

        // Försök att placera en insats. Returnerar true om det lyckas, false om det är ogiltigt.
        public bool PlaceBet(int amount)
        {
            if (amount <= 0 || amount > Chips)
            {
                return false; // Insatsen får inte vara 0, negativ eller större än tillgängliga marker
            }

            CurrentBet = amount;
            Chips -= amount; // Dra av insatsen från saldot direkt när den placeras
            return true;
        }

        // Hantera vinst 1:1 (Insatsen tillbaka + lika mycket till)
        public void WinBet()
        {
            Chips += CurrentBet * 2;
            CurrentBet = 0;
        }

        // Hantera Blackjack 3:2 (Insatsen tillbaka + 1.5 gånger insatsen i vinst)
        public void BlackjackPayout()
        {
            // Ex: 100kr insats ger 100kr tillbaka + 150kr vinst = 250kr totalt
            int winnings = (int)(CurrentBet * 1.5);
            Chips += CurrentBet + winnings;
            CurrentBet = 0;
        }

        // Hantera Lika / Push (Insatsen går tillbaka oavkortat)
        public void PushBet()
        {
            Chips += CurrentBet;
            CurrentBet = 0;
        }

        // Hantera Förlust (Insatsen är redan borta från saldot, nollställ bara CurrentBet)
        public void LoseBet()
        {
            CurrentBet = 0;
        }
        public void RecordOutcome(Outcome outcome)
        {
            switch (outcome)
            {
                case Outcome.Win:
                    Wins++;
                    WinBet();
                    break;
                case Outcome.Lose:
                    Losses++;
                    LoseBet();
                    break;
                case Outcome.Push:
                    Pushes++;
                    PushBet();
                    break;
                case Outcome.Blackjack:
                    Blackjacks++;
                    BlackjackPayout();
                    break;
            }
        }

        public void SetStats(int wins, int losses, int pushes, int blackjacks, int chips)
        {
            Wins = wins;
            Losses = losses;
            Pushes = pushes;
            Blackjacks = blackjacks;
            Chips = chips;
        }
    }

}
