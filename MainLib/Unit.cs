using InterpreterLib;
namespace MainLib
{
    public class Unit : Card, IAttackable
    {
        public override string Name { get; }
        public override string Description { get; }
        public override int Cost { get; }
        protected override AST ast { get; }
        public int Damage { get; set; }
        public int Defense { get; set; }
        public void Attack(IAttackable targetCard)
        {
            Defense -= targetCard.Damage;
            targetCard.Defense -= Damage;
        }
        public Unit(string name, int cost, int damage, int defense, string order, string description)
        {
            Name = name;
            Cost = cost;
            Damage = damage;
            Defense = defense;
            Description = description;

            // Building AST and allowing a unit card to carry no order
            if (order is not "")
            {
                Lexer lexer = new Lexer(order);
                Parser parser = new Parser(lexer);
                ast = parser.Parse();
            }
            else
            {
                ast = null;
            }
        }
    }
}