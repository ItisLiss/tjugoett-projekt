using System;
using System.Collections.Generic;

namespace Kunskapskontroll3
{
    public class Game
    {
        private Deck _deck;
        private List<Player> _players;
        private Dealer _dealer;

        public Game(List<Player> players)
        {
            _players = players;
            _deck = new Deck();
            _dealer = new Dealer();
        }

        // 1. Starta en ny runda och dela ut startkort
        public void StartNewRound()
        {
            _deck = new Deck(); // Skapa och blanda en ny kortlek
            _deck.Shuffle();     // Anta att din Shuffle-metod heter så eller körs i konstruktorn

            // Nollställ spelarnas händer
            foreach (var player in _players)
            {
                player.NewHand();
            }
            _dealer.NewHand();

            // Dela ut 2 kort till varje spelare via ReceiveCard
            foreach (var player in _players)
            {
                player.ReceiveCard(_deck.DealCard());
                player.ReceiveCard(_deck.DealCard());
            }

            // Dela ut 2 kort till dealern
            _dealer.ReceiveCard(_deck.DealCard());
            _dealer.ReceiveCard(_deck.DealCard());
        }

        // 2. Låta en spelare dra ett kort (Hit)
        public void PlayerHit(Player player)
        {
            player.ReceiveCard(_deck.DealCard());
        }
    }
}