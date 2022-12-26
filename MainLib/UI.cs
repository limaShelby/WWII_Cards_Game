namespace MainLib
{
    public static class UI
    {
        //** All this code is responsible for displaying the whole game to the user/users
        public static void Intro()
        {
            Console.Clear();
            Console.WriteLine("Welcome to WWII Cards Game");
            Console.Write("> ");
        }
        public static void ShowGame(Player currentPlayer, Player opponentPlayer)
        {
            PrintFields(currentPlayer, opponentPlayer);
            PrintHand(currentPlayer);
            PrintHQs(currentPlayer, opponentPlayer);
            PrintCredits(currentPlayer, opponentPlayer);
            PrintDecks(currentPlayer, opponentPlayer);
        }
        public static void PrintDescription(string description)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine(description);
            Console.ResetColor();
        }
        public static void ErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cannot execute that order");
            Console.ResetColor();
        }
        public static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cannot execute that order. " + message);
            Console.ResetColor();
        }
        private static void PrintHand(Player currentPlayer)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            System.Console.WriteLine("Your hand:");
            Console.ResetColor();
            for (int i = 0; i < currentPlayer.Hand.Count; i++)
            {
                if (currentPlayer.Hand[i] is Unit unit)
                {
                    System.Console.WriteLine($"H{i}- Name: " + unit.Name + " | Damage: " + unit.Damage + " | Defense: " + unit.Defense + " | Cost: " + unit.Cost);
                }
                else
                {
                    System.Console.WriteLine($"H{i}- Name: " + currentPlayer.Hand[i].Name + " | Cost: " + currentPlayer.Hand[i].Cost);
                }
            }
        }
        private static void PrintFields(Player currentPlayer, Player opponentPlayer)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("Your field:");
            Console.ResetColor();
            for (int i = 0; i < currentPlayer.Field.Length; i++)
            {
                FieldPosition position = currentPlayer.Field[i];
                if (position.IsEmpty())
                {
                    System.Console.WriteLine($"F{i}- Empty Position");
                }
                else
                {
                    System.Console.WriteLine($"F{i}- Name: " + position.Card.Name + " | Damage: " + position.Card.Damage + " | Defense: " + position.Card.Defense + " | Cost: " + position.Card.Cost + " | " + position.HasAttacked + " | " + position.HasPerformed);
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("Your opponent's field:");
            Console.ResetColor();
            for (int i = 0; i < opponentPlayer.Field.Length; i++)
            {
                FieldPosition position = opponentPlayer.Field[i];
                if (position.IsEmpty())
                {
                    Console.WriteLine($"O{i}- Empty Position");
                }
                else
                {
                    System.Console.WriteLine($"O{i}- Name: " + position.Card.Name + " | Damage: " + position.Card.Damage + " | Defense: " + position.Card.Defense + " | Cost: " + position.Card.Cost + " | " + position.HasAttacked + " | " + position.HasPerformed);
                }
            }
        }
        private static void PrintHQs(Player currentPlayer, Player waitingPlayer)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Your HQ: " + currentPlayer.HQ.Defense);
            System.Console.WriteLine("Opponent HQ: " + waitingPlayer.HQ.Defense);
            Console.ResetColor();
        }
        private static void PrintCredits(Player currentPlayer, Player opponentPlayer)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine("Your Credits: " + currentPlayer.Credits);
            System.Console.WriteLine("Opponent Credits: " + opponentPlayer.Credits);
            Console.ResetColor();
        }
        private static void PrintDecks(Player currentPlayer, Player opponentPlayer)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine("Cards left on your deck: " + currentPlayer.Deck.Count);
            System.Console.WriteLine("Cards left on your opponent's deck: " + opponentPlayer.Deck.Count);
            Console.ResetColor();
        }
    }
}