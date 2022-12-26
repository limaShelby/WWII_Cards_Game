namespace MainLib
{
    public class AI_Player : Player
    {
        //* Initializing "Deck" and "Name"
        public override List<Card> Deck { get; set; }
        public override string Name { get; }
        public AI_Player(string name, List<Card> deck)
        {
            Deck = deck;
            Name = name;
        }

        // Implementing "Play()"
        public override void Play(Player opponent)
        {
            // Performing all attacks possible
            while (!OrderAttacks(opponent))
            {
                RemoveCards();
                opponent.RemoveCards();
            }

            // Deploying all cards possible
            while (!OrderDeployment(opponent)) { }

            // Performing all cards possible
            OrderPerformances(opponent);

            //* Removing destroyed cards from fields
            RemoveCards();
            opponent.RemoveCards();
        }
        private void OrderPerformances(Player opponent)
        {
            for (int i = 0; i < Field.Length; i++)
            {
                //* Validating the selected card
                if (Field[i].IsEmpty() || Field[i].Card.Cost > Credits || Field[i].HasPerformed) continue;

                ExecutePerformance(Field[i].Card, Field[i], this, opponent);
            }
        }
        private bool OrderAttacks(Player opponent)
        {
            //* If HQ is alone, then attack it with all you've got
            if (IsHQAlone(opponent.Field))
            {
                for (int i = 0; i < Field.Length; i++)
                {
                    // Validating the selected card
                    if (Field[i].IsEmpty() || Field[i].HasAttacked || Field[i].Card.Cost > Credits) continue;

                    // Executing the attack
                    ExecuteAttack(Field[i], opponent.HQ);
                    return false;
                }
            }
            else
            {
                //* If HQ is not alone, then attack all cards possible
                for (int i = 0; i < Field.Length; i++)
                {
                    for (int j = 0; j < opponent.Field.Length; j++)
                    {
                        //* Validating the selected card and the target card
                        if (Field[i].IsEmpty() || Field[i].HasAttacked || Field[i].Card.Cost > Credits) break;
                        if (opponent.Field[j].IsEmpty()) continue;

                        // Executing the attack
                        ExecuteAttack(Field[i], opponent.Field[j].Card);
                        return false;
                    }
                }
            }
            return true;
        }
        private bool OrderDeployment(Player opponent)
        {
            foreach (Card card in Hand)
            {
                // If card is an "order", then perform it if credits are enough
                if (card is Order order)
                {
                    if (order.Cost > Credits) continue;

                    ExecutePerformance(order, this, opponent);
                    return false;
                }

                // If card is a unit, there's available space and credits are enough, then deploy it
                for (int i = 0; i < Field.Length; i++)
                {
                    if (!Field[i].IsEmpty()) continue;

                    if (card.Cost > Credits) break;

                    ExecuteDeployment(card as Unit, Field[i]);

                    return false;
                }
            }

            return true;
        }
    }
}