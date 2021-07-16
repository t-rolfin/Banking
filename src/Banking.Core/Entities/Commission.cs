namespace Banking.Core.Entities
{
    public class Commission
    {
        public Commission(float percent, float fixedValue)
        {
            Percent = percent;
            FixedValue = fixedValue;
        }

        public float Percent { get; set; }
        public float FixedValue { get; set; }
    }
}
