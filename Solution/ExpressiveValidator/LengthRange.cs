namespace ExpressiveValidator
{
    public struct LengthRange
    {
        public int Min { get; }
        public int Max { get; }

        public LengthRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"{Min}-{Max}";
        }
    }
}
