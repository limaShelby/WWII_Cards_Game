namespace MainLib
{
    public class FieldPosition
    {
        public Unit Card { get; set; } = null;
        public bool HasAttacked { get; set; } = false;
        public bool HasPerformed { get; set; } = false;
        public bool IsEmpty()
        {
            return Card == null;
        }
    }
}