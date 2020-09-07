namespace Assets.Scripts.Characters
{
    public interface IDamageCalculator
    {
        float Calculate(float amount, ref float armor);
    }
}