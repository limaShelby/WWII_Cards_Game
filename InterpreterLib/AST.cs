namespace InterpreterLib
{
    public abstract class AST { };

    public class Statement : AST
    {
        public Statement(AST left, AST right)
        {
            Left = left;
            Right = right;
        }

        public AST Left { get; }
        public AST Right { get; }
    }
    public class UnaryAction : AST
    {
        public UnaryAction(Token token, AST selector, Token integer)
        {
            Token = token;
            Selector = selector;
            Integer = integer;
            Value = token.Value;
        }
        public Token Token { get; }
        public AST Selector { get; }
        public Token Integer { get; private set; }
        public string Value { get; private set; }
    }
    public class HQ_Action : AST
    {
        public HQ_Action(Token token, Token integer)
        {
            Token = token;
            Integer = integer;
            Value = token.Value;
        }
        public Token Token { get; }
        public Token Integer { get; }
        public string Value { get; private set; }
    }
    public class CreditsAction : AST
    {
        public CreditsAction(Token token, Token integer)
        {
            Token = token;
            Integer = integer;
            Value = token.Value;
        }
        public Token Token { get; }
        public Token Integer { get; }
        public string Value { get; private set; }
    }
    public class SelectedCard : AST
    {
        public SelectedCard(Token token)
        {
            Token = token;
            Value = token.Value;
        }
        public Token Token { get; }
        public string Value { get; private set; }
    }
}