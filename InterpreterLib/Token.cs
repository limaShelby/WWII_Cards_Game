namespace InterpreterLib
{
    public class Token
    {
        public Token(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; }
        public string Value { get; }
    }
}