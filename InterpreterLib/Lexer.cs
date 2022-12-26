namespace InterpreterLib
{
    public class Lexer
    {
        static string Text;
        static int Position;
        static char CurrentChar;
        public Token CurrentToken { get; set; }
        public Lexer(string text)
        {
            Text = text;
            Position = 0;
            CurrentChar = text[Position];
            CurrentToken = null;
        }
        private Token GetWordToken()
        {
            string result = "";

            // While currentChar is not an end-char and is a letter or digit, add it up to result and move on
            while (CurrentChar != '@' && char.IsLetterOrDigit(CurrentChar))
            {
                result += CurrentChar;
                Advance();
            }

            // Building tokens according to Syntax
            switch (result)
            {
                case Syntax.OwnerMax:
                    return new Token(Syntax.OwnerMax, result);
                case Syntax.OwnerMin:
                    return new Token(Syntax.OwnerMin, result);
                case Syntax.OpponentMax:
                    return new Token(Syntax.OpponentMax, result);
                case Syntax.OpponentMin:
                    return new Token(Syntax.OpponentMin, result);
                case Syntax.IncreaseDamage:
                    return new Token(Syntax.IncreaseDamage, result);
                case Syntax.DecreaseDamage:
                    return new Token(Syntax.DecreaseDamage, result);
                case Syntax.IncreaseDefense:
                    return new Token(Syntax.IncreaseDefense, result);
                case Syntax.DecreaseDefense:
                    return new Token(Syntax.DecreaseDefense, result);
                case Syntax.IncreaseHQ:
                    return new Token(Syntax.IncreaseHQ, result);
                case Syntax.IncreaseCredits:
                    return new Token(Syntax.IncreaseCredits, result);
                case Syntax.By:
                    return new Token(Syntax.By, result);
                case Syntax.Self:
                    return new Token(Syntax.Self, result);
                default:
                    throw new Exception("Error parsing input");
            }
        }
        private static void Advance()
        {
            if (Position >= Text.Length - 1)
            {
                CurrentChar = '@';
            }
            else
            {
                // If string isn't over, then move onto the next char
                Position++;
                CurrentChar = Text[Position];
            }
        }
        private string GetInteger()
        {
            string result = CurrentChar.ToString();

            // While char is a digit, keep on building result
            Advance();
            while (char.IsDigit(CurrentChar))
            {
                result += CurrentChar;
                Advance();
            }

            return result;
        }
        private void SkipWhiteSpace()
        {
            // While it's not over and char is a white space, move on
            while (CurrentChar != '@' && char.IsWhiteSpace(CurrentChar))
            {
                Advance();
            }
        }
        private string GetSymbol(char op)
        {
            if (op == '(') return Syntax.LParenth;
            else if (op == ')') return Syntax.RParenth;
            else if (op == ';') return Syntax.SemiColon;
            else throw new ArgumentException();
        }

        public Token GetNextToken()
        {
            // Skiping white space
            if (char.IsWhiteSpace(CurrentChar))
            {
                SkipWhiteSpace();
            }

            // Building EOF token
            if (CurrentChar == '@')
            {
                return new Token(Syntax.EOF, "@");
            }

            // Building Integer Token
            if (char.IsDigit(CurrentChar))
            {
                return new Token(Syntax.Integer, GetInteger());
            }

            // Building SEMI or PAREN token
            if (CurrentChar == ';' || CurrentChar == '(' || CurrentChar == ')')
            {
                char temporaryChar = CurrentChar;
                Advance();
                return new Token(GetSymbol(temporaryChar), temporaryChar.ToString());
            }

            // Building any other available token according to Syntax
            if (char.IsLetter(CurrentChar)) return GetWordToken();

            throw new Exception("Error parsing input");
        }
    }
    public struct Syntax
    {
        public const string IncreaseDamage = "IncreaseDamage";
        public const string DecreaseDamage = "DecreaseDamage";
        public const string IncreaseDefense = "IncreaseDefense";
        public const string DecreaseDefense = "DecreaseDefense";
        public const string OwnerMax = "OwnerMax";
        public const string OwnerMin = "OwnerMin";
        public const string OpponentMax = "OpponentMax";
        public const string OpponentMin = "OpponentMin";
        public const string IncreaseHQ = "IncreaseHQ";
        public const string IncreaseCredits = "IncreaseCredits";
        public const string Self = "Self";
        public const string LParenth = "LParenth";
        public const string RParenth = "RParenth";
        public const string Integer = "Integer";
        public const string By = "By";
        public const string Max = "Max";
        public const string Min = "Min";
        public const string SemiColon = "SemiColon";
        public const string EOF = "EOF";
    }
}