namespace Kunskapskontroll3
{
    class CardRenderer
    {
        public string[] Render(Card card)
        {
            string rankSymbol;

            switch (card.Rank)
            {
                case Rank.Ace:
                    rankSymbol = "A";
                    break;
                case Rank.King:
                    rankSymbol = "♚";
                    break;
                case Rank.Queen:
                    rankSymbol = "♛";
                    break;
                case Rank.Jack:
                    rankSymbol = "♞";
                    break;
                default:
                    rankSymbol = ((int)card.Rank).ToString();
                    break;
            }

            string suitSymbol;

            switch (card.Suit)
            {
                case Suit.Hearts:
                    suitSymbol = "♥";
                    break;
                case Suit.Diamonds:
                    suitSymbol = "♦";
                    break;
                case Suit.Clubs:
                    suitSymbol = "♣";
                    break;
                case Suit.Spades:
                    suitSymbol = "♠";
                    break;
                default:
                    suitSymbol = "?";
                    break;
            }

            return new[]
            {
                "┌───────┐",
                $"│{rankSymbol.PadRight(7)}│",
                "│       │",
                $"│   {suitSymbol}   │",
                "│       │",
                $"│{rankSymbol.PadLeft(7)}│",
                "└───────┘"
            };
        }

        public string[] RenderBack()
        {
            return new[]
            {
                "┌───────┐",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "└───────┘"
            };
        }
    }
}
