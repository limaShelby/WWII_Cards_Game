using InterpreterLib;
namespace MainLib
{
    public class Order : Card
    {
        public override string Name { get; }
        public override string Description { get; }
        public override int Cost { get; }
        protected override AST ast { get; }
        public Order(string name, int cost, string order, string description)
        {
            Name = name;
            Cost = cost;
            Description = description;

            // Building the ast
            Lexer lexer = new Lexer(order);
            Parser parser = new Parser(lexer);
            ast = parser.Parse();
        }
    }
}