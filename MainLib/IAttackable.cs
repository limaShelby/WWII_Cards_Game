namespace MainLib
{
    public interface IAttackable
    {
        int Damage { get; }
        int Defense { get; set; }
    }
}