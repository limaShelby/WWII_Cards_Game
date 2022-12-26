using MainLib;

namespace ConsoleUI
{
    public class Manager
    {
        public Manager(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            // Setting round to zero
            Round = 0;

            // Setting initial credits to five
            Player1.Credits = 5;
            Player2.Credits = 5;
        }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        private int Round;

        // This method manages the game
        public string Start()
        {
            // Each loop is a game's round
            while (true)
            {
                // Distributing cards to players
                DistributeCards();

                // Checking if one of the player has lost
                if (IsGameOver()) break;

                //* Allowing players to play and restoring the "hasAttacked" and "hasPerformed" properties of "positionFields"
                Player1.Play(Player2);
                UpdateField(Player1);
                Player2.Play(Player1);
                UpdateField(Player2);

                // Moving up to the next round
                Round++;
                // Distributing the credits (The amount increases per round, until it reaches 12)
                DistributeCredits();
            }

            // Returning winner's message
            if (Player1.IsOutOfCards() || Player1.HQ.Defense <= 0) return $"{Player2.Name} has won!";
            else return $"{Player1.Name} has won!";
        }

        // This method restores the "hasAttacked" and "hasPerformed" properties of "positionFields"
        private void UpdateField(Player player)
        {
            for (int i = 0; i < player.Field.Length; i++)
            {
                player.Field[i].HasAttacked = false;
                player.Field[i].HasPerformed = false;
            }
        }

        // This method distributes cards to each players using the "GetCards()" method
        private void DistributeCards()
        {
            if (Round == 0)
            {
                GetCards(6, Player1);
                GetCards(6, Player2);
                return;
            }

            GetCards(2, Player1);
            GetCards(2, Player2);
        }
        private void GetCards(int n, Player player)
        {
            int cardsToDistribute = n;

            // If the amount of cards left in the deck is lesser than the amount to distribute, then decrease the latter until they're both equal.
            while (player.Deck.Count - cardsToDistribute < 0)
            {
                cardsToDistribute--;
            }

            // Making the transaction from player's deck to his/her hand
            for (int i = 0; i < cardsToDistribute; i++)
            {
                Card tempCard = player.Deck[0];
                player.Deck.RemoveAt(0);
                player.Hand.Add(tempCard);
            }
        }

        // This method distributes credits to each player
        private void DistributeCredits()
        {
            // While Round is lesser than or equal to 12, then increase the credits along the rounds
            if (Round <= 12)
            {
                Player1.Credits += 1 + Round;
                Player2.Credits += 1 + Round;
                return;
            }
            // If Round is greater than 12, hold on to 12
            Player1.Credits += 1 + 12;
            Player2.Credits += 1 + 12;
        }

        // This method checks if one of the players has lost
        private bool IsGameOver()
        {
            // A player looses if he/she ran out of cards or it's HQ has been destroyed
            return Player1.IsOutOfCards() || Player2.IsOutOfCards() || Player1.HQ.Defense <= 0 || Player2.HQ.Defense <= 0;
        }
    }
}