namespace MainLib
{
    public class HeadQuarter : IAttackable
    {
        public int Damage { get; private set; } = 0;
        public int Defense { get; set; }
        public HeadQuarter(int defense)
        {
            Defense = defense;
        }
    }
}