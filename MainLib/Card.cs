using InterpreterLib;
namespace MainLib
{
    public abstract class Card
    {
        public abstract string Name { get; }
        public abstract int Cost { get; }
        public abstract string Description { get; }
        protected abstract AST ast { get; }

        // Executing AST
        public bool Perform(Player owner, Player opponent)
        {
            return Execute_AST(ast, owner, opponent);
        }
        private bool Execute_AST(AST node, Player owner, Player opponent)
        {
            if (ast == null) return false;

            if (node is Statement statement)
            {
                // Moving through left and right child of the statement node
                Execute_AST(statement.Left, owner, opponent);
                Execute_AST(statement.Right, owner, opponent);
            }
            else if (node is HQ_Action hq_Action)
            {
                // Executing "HQ_Action" node
                owner.HQ.Defense += int.Parse(hq_Action.Integer.Value);
            }
            else if (node is CreditsAction creditsAction)
            {
                // Executing "CreditsAction" node
                owner.Credits += int.Parse(creditsAction.Integer.Value);
            }
            else if (node is UnaryAction unaryAction)
            {
                // Getting the card encoded in the selector
                Unit card = GetCard(unaryAction.Selector, owner
                , opponent);

                // If there's no such card return false
                if (card == null) return false;
                int value = int.Parse(unaryAction.Integer.Value);

                //* Executing "UnaryAction" nodes
                if (unaryAction.Value == Syntax.DecreaseDamage)
                {
                    if (card.Damage - value < 0) card.Damage = 0;
                    else card.Damage -= value;
                }
                else if (unaryAction.Value == Syntax.IncreaseDamage)
                {
                    card.Damage += value;
                }
                else if (unaryAction.Value == Syntax.IncreaseDefense)
                {
                    card.Defense += value;
                }
                else if (unaryAction.Value == Syntax.DecreaseDefense)
                {
                    card.Defense -= value;
                }
            }
            return true;
        }

        // Translating the selector into an actual card
        private Unit GetCard(AST selector, Player owner, Player opponent)
        {
            string cardInfo = ((SelectedCard)selector).Value;

            switch (cardInfo)
            {
                case Syntax.OwnerMax:
                    return GetMax(owner);
                case Syntax.OwnerMin:
                    return GetMin(owner);
                case Syntax.OpponentMax:
                    return GetMax(opponent);
                case Syntax.OpponentMin:
                    return GetMin(opponent);
                case Syntax.Self:
                    if (this is Unit unit) return unit;
                    else break;
            }
            return null;
        }

        // Getting the current max-damage card on a player's field
        private Unit GetMax(Player player)
        {
            Unit max = (player.Field[0].Card is null) ? null : player.Field[0].Card;

            for (int i = 0; i < player.Field.Length; i++)
            {
                if (player.Field[i].IsEmpty()) continue;
                Unit temp = player.Field[i].Card;
                if (max is null)
                {
                    max = temp;
                    continue;
                }
                if (max.Damage < temp.Damage) max = temp;
            }

            return max;
        }

        // Getting the current min-damage card on a player's field
        private Unit GetMin(Player player)
        {
            Unit min = (player.Field[0].Card is null) ? null : player.Field[0].Card;

            for (int i = 0; i < player.Field.Length; i++)
            {
                if (player.Field[i].IsEmpty()) continue;
                Unit temp = player.Field[i].Card;
                if (min is null)
                {
                    min = temp;
                    continue;
                }
                if (min.Damage > temp.Damage) min = temp;
            }

            return min;
        }
    }
}