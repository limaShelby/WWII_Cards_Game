namespace MainLib
{
    public class UserPlayer : Player
    {
        // Initializing "Deck" and "Name"
        public override List<Card> Deck { get; set; }
        public override string Name { get; }
        public UserPlayer(string name, List<Card> deck)
        {
            Deck = deck;
            Name = name;
        }

        // Implementing "Play()"
        public override void Play(Player opponent)
        {
            // Executing each user's input until he/she decides to end his/her turn
            while (true)
            {
                // If opponent's HQ has been destroyed then it's over
                if (opponent.HQ.Defense <= 0) return;

                // Displaying game to user
                UI.ShowGame(this, opponent);

                // Getting input
                Console.ForegroundColor = ConsoleColor.Yellow;
                string order = Console.ReadLine();
                Console.ResetColor();

                Console.Clear();
                // If input equals "e" then end turn
                if (order == "e") return;

                // Executing user's order
                ExecuteOrder(order, opponent);

                //* Removing destroyed cards from fields
                RemoveCards();
                opponent.RemoveCards();
            }
        }
        private void ExecuteOrder(string order, Player opponent)
        {
            // Spliting words
            string[] words = order.Split(' ');

            //* Validating input
            if (words.Length == 3)
            {
                if (words[1] == "attack" || words[1] == "a")
                {
                    // Processing input coordinate
                    int chosenCardCoord = ProcessCoordinates(words[0], out char chosenFieldCoord);
                    // Validating input coordinate
                    if (chosenFieldCoord != 'f' || chosenCardCoord < 0 || chosenCardCoord >= Field.Length)
                    {
                        UI.ErrorMessage("Wrong coordinates");
                        return;
                    }
                    // Selecting position
                    FieldPosition selectedField1 = Field[chosenCardCoord];

                    // Validating selected position
                    if (selectedField1.IsEmpty() || selectedField1.Card.Cost > Credits || selectedField1.HasAttacked)
                    {
                        UI.ErrorMessage("Check if the position you've selected is empty, if the card you've chosen has already attacked or if your credits are not enough.");
                        return;
                    }

                    // Validating for an "HQ attack"
                    if ((words[2] == "HQ" || words[2] == "hq") && IsHQAlone(opponent.Field))
                    {
                        ExecuteAttack(selectedField1, opponent.HQ);
                    }
                    else
                    {
                        // Processing input coordinate
                        chosenCardCoord = ProcessCoordinates(words[2], out chosenFieldCoord);
                        // Validating input coordinate
                        if (chosenFieldCoord != 'o' || chosenCardCoord < 0 || chosenCardCoord > opponent.Field.Length)
                        {
                            UI.ErrorMessage("Wrong coordinates");
                            return;
                        }
                        // Selecting position
                        FieldPosition selectedField2 = opponent.Field[chosenCardCoord];

                        // Validating position
                        if (selectedField2.IsEmpty())
                        {
                            UI.ErrorMessage("You've selected an empty position");
                            return;
                        }

                        // Performing the order
                        ExecuteAttack(selectedField1, selectedField2.Card);
                    }
                }
                else
                {
                    UI.ErrorMessage("The order is misspelt.");
                }
            }
            else if (words.Length == 2)
            {
                // Validating for a deployment
                if (words[0] == "deploy" || words[0] == "dp")
                {
                    // Processing input coordinates
                    int chosenCardCoord = ProcessCoordinates(words[1], out char chosenFieldCoord);
                    // Validating input coordinates
                    if (chosenCardCoord < 0 || chosenCardCoord >= Hand.Count || chosenFieldCoord != 'h')
                    {
                        UI.ErrorMessage("Wrong coordinates");
                        return;
                    }
                    // Selecting card
                    Card card = Hand[chosenCardCoord];

                    // Validating if credits are enough
                    if (card.Cost > Credits)
                    {
                        UI.ErrorMessage("Not enough credits to execute deployment");
                        return;
                    }

                    // If card is a unit and there's available space, then deploy it
                    if (card is Unit unit)
                    {
                        for (int i = 0; i < Field.Length; i++)
                        {
                            if (!Field[i].IsEmpty()) continue;

                            // Performing the order
                            ExecuteDeployment(unit, Field[i]);

                            return;
                        }
                    }
                    else
                    {
                        // If card is an order, then perform it
                        ExecutePerformance(card as Order, this, opponent);
                        return;
                    }
                }
                else if (words[0] == "perform" || words[0] == "p")
                {
                    // Processing coordinates
                    int chosenCardCoord = ProcessCoordinates(words[1], out char chosenFieldCoord);
                    // Validating coordinates
                    if (chosenFieldCoord != 'f' || chosenCardCoord < 0 || chosenCardCoord >= Field.Length)
                    {
                        UI.ErrorMessage("Wrong coordinates");
                        return;
                    }
                    // Selecting position
                    FieldPosition selectedPosition = Field[chosenCardCoord];

                    // Validating position
                    if (selectedPosition.IsEmpty() || selectedPosition.Card.Cost > Credits || selectedPosition.HasPerformed)
                    {
                        UI.ErrorMessage("Check if the position you've selected is empty, or if the card you've chosen has already performed or if your credits are not enough.");
                        return;
                    }

                    // Executing order
                    ExecutePerformance(selectedPosition.Card, selectedPosition, this, opponent);
                }
                else if (words[0] == "description" || words[0] == "ds")
                {
                    // Processing coordinates
                    int chosenCardCoord = ProcessCoordinates(words[1], out char chosenFieldCoord);

                    //** Validating coordint
                    if (chosenCardCoord < 0)
                    {
                        UI.ErrorMessage("Wrong coordinates");
                        return;
                    }
                    if (chosenFieldCoord is 'f')
                    {
                        if (chosenCardCoord > Field.Length || Field[chosenCardCoord].IsEmpty())
                        {
                            UI.ErrorMessage("Wrong coordinates");
                            return;
                        }
                        // Executing order
                        UI.PrintDescription(Field[chosenCardCoord].Card.Description);
                    }
                    else if (chosenFieldCoord is 'o')
                    {
                        if (chosenCardCoord > opponent.Field.Length || opponent.Field[chosenCardCoord].IsEmpty())
                        {
                            UI.ErrorMessage("Wrong coordinates");
                            return;
                        }
                        // Executing order
                        UI.PrintDescription(opponent.Field[chosenCardCoord].Card.Description);
                    }
                    else if (chosenFieldCoord is 'h')
                    {
                        if (chosenCardCoord >= Hand.Count)
                        {
                            UI.ErrorMessage("Wrong coordinates");
                            return;
                        }
                        // Executing order
                        UI.PrintDescription(Hand[chosenCardCoord].Description);
                    }
                    else
                    {
                        UI.ErrorMessage("Wrong coordinates");
                    }
                }
                else
                {
                    UI.ErrorMessage("The order is misspelt.");
                }
            }
            else
            {
                UI.ErrorMessage();
            }
        }

        // This method process input coordinates
        private static int ProcessCoordinates(string coordinates, out char playerField)
        {
            playerField = ' ';

            //* If coordinates are not valid return -1
            if (coordinates.Length != 2) return -1;
            if (!char.IsDigit(coordinates[1])) return -1;

            // Return valid values
            playerField = coordinates[0];
            return int.Parse(coordinates[1].ToString());
        }
    }
}