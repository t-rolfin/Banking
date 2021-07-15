namespace Banking.Core.Entities
{
    public class Commission
    {
        public Commission(float percent, float @fixed)
        {
            Percent = percent;
            Fixed = @fixed;
        }

        public float Percent { get; set; }
        public float Fixed { get; set; }
    }
}
