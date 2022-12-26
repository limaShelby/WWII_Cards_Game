namespace MainLib
{
    public abstract class Player
    {
        //* This two properties must be initialized by descendant classes
        public abstract List<Card> Deck { get; set; }
        public abstract string Name { get; }

        //* Initializing properties
        public List<Card> Hand = new List<Card>();

        public FieldPosition[] Field = new FieldPosition[5]
        {
            new FieldPosition {Card = null, HasAttacked = false},
            new FieldPosition {Card = null, HasAttacked = false},
            new FieldPosition {Card = null, HasAttacked = false},
            new FieldPosition {Card = null, HasAttacked = false},
            new FieldPosition {Card = null, HasAttacked = false}
        };
        public int Credits { get; set; } = 0;
        public HeadQuarter HQ = new HeadQuarter(100);

        // This method must be implemented by its descendant classes
        public abstract void Play(Player opponent);

        // This method removes destroyed cards from the field
        public void RemoveCards()
        {
            for (int i = 0; i < Field.Length; i++)
            {
                if (Field[i].IsEmpty()) continue;
                if (Field[i].Card.Defense <= 0) Field[i].Card = null;
            }
        }

        // This method allows a player to perfom an attack
        protected void ExecuteAttack(FieldPosition selectedCard, IAttackable target)
        {
            selectedCard.Card.Attack(target);
            selectedCard.HasAttacked = true;
            Credits -= selectedCard.Card.Cost;
        }

        // This method allows a player to deploy a unit
        protected void ExecuteDeployment(Unit selectedCard, FieldPosition selectedPosition)
        {
            Unit tempUnit = selectedCard;
            Hand.Remove(selectedCard);
            selectedPosition.Card = tempUnit;
            selectedPosition.HasAttacked = true;
            Credits -= selectedCard.Cost;
        }

        // This method allows a player to perform a order card
        protected void ExecutePerformance(Order selectedCard, Player owner, Player opponent)
        {
            selectedCard.Perform(owner, opponent);
            Credits -= selectedCard.Cost;
            owner.Hand.Remove(selectedCard);
        }

        // This method allows a player to perform a unit card
        protected void ExecutePerformance(Unit selectedCard, FieldPosition selectedPosition, Player owner, Player opponent)
        {
            if (selectedCard.Perform(owner, opponent))
            {
                Credits -= selectedCard.Cost;
                selectedPosition.HasPerformed = true;
            }
        }

        // This method checks if the player has ran out of cards
        public bool IsOutOfCards()
        {
            return Deck.Count == 0 && Hand.Count == 0 && IsHQAlone(Field);
        }

        // This method checks if there are no units on a field
        protected bool IsHQAlone(FieldPosition[] playerField)
        {
            for (int i = 0; i < playerField.Length; i++)
            {
                if (!playerField[i].IsEmpty()) return false;
            }

            return true;
        }
    }
}