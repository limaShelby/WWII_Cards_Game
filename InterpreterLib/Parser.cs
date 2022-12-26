namespace InterpreterLib
{
    public class Parser
    {
        Lexer _lexer;
        Token CurrentToken;
        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            CurrentToken = _lexer.GetNextToken();
        }
        private void Eat(string tokenType)
        {
            // Validating token's type
            if (CurrentToken.Type == tokenType) CurrentToken = _lexer.GetNextToken();
            else throw new Exception("Error parsing input");
        }
        private AST Selector()
        {
            Token token = CurrentToken;

            // Building SelectedCard node according to Syntax
            switch (token.Type)
            {
                case Syntax.OwnerMax:
                    Eat(Syntax.OwnerMax);
                    return new SelectedCard(token);
                case Syntax.OwnerMin:
                    Eat(Syntax.OwnerMin);
                    return new SelectedCard(token);
                case Syntax.OpponentMax:
                    Eat(Syntax.OpponentMax);
                    return new SelectedCard(token);
                case Syntax.OpponentMin:
                    Eat(Syntax.OpponentMin);
                    return new SelectedCard(token);
                case Syntax.Self:
                    Eat(Syntax.Self);
                    return new SelectedCard(token);
                default:
                    throw new Exception("Error parsing input");
            }
        }
        private AST Expression()
        {
            Token token = CurrentToken;

            // Checking if it start with an UnaryAction's token
            if (token.Type == Syntax.IncreaseDamage || token.Type == Syntax.DecreaseDamage || token.Type == Syntax.IncreaseDefense || token.Type == Syntax.DecreaseDefense)
            {
                Eat(token.Type);

                // Checking for parenthesis and building selector
                Eat(Syntax.LParenth);
                AST node = Selector();
                Eat(Syntax.RParenth);

                // Checking for By
                Eat(Syntax.By);

                // Checking for integer value
                Token integer = CurrentToken;
                Eat(Syntax.Integer);

                // Building UnaryAction node
                return new UnaryAction(token, node, integer);
            }
            // Checking for HQ_Action's token
            else if (token.Type == Syntax.IncreaseHQ)
            {
                Eat(token.Type);

                // Checking for By
                Eat(Syntax.By);

                // Checking for integer value
                Token integer = CurrentToken;
                Eat(Syntax.Integer);

                // Building a HQ_Action node
                return new HQ_Action(token, integer);
            }
            // Checking for a CreditsAction's token
            else if (token.Type == Syntax.IncreaseCredits)
            {
                Eat(token.Type);

                // Checking for By
                Eat(Syntax.By);

                // Checking for integer value
                Token integer = CurrentToken;
                Eat(Syntax.Integer);

                // Building a CreditsAction node
                return new CreditsAction(token, integer);
            }
            else
            {
                throw new Exception("Error parsing input");
            }
        }
        private AST Statement()
        {
            // Building a expression
            AST node = Expression();
            // Checking for semicolon
            Eat(Syntax.SemiColon);

            // Checking for another statement
            while (CurrentToken.Type.Contains("Increase") || CurrentToken.Type.Contains("Decrease"))
            {
                return new Statement(node, Statement());
            }

            return node;
        }
        public AST Parse()
        {
            // Building AST
            AST node = Statement();
            if (CurrentToken.Type != Syntax.EOF) throw new Exception("Error parsing input");

            return node;
        }
    }
}