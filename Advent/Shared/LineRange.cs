namespace Advent.Shared
{
    public struct LineRange
    {
        public int start;
        public int end;

        public readonly int Length => end - start;

        public LineRange(int start, int length)
        {
            this.start = start;
            this.end = start + length;
        }

        public static LineRange operator +(int a, LineRange b) => new LineRange(a + b.start, a + b.end);
        public static LineRange operator +(LineRange b, int a) => new LineRange(a + b.start, a + b.end);

        public bool Overlaps(LineRange other)
        {
            if (other.end <= start || end <= other.start)
                return false;
            return true;
        }

        public static LineRange FromLength(int start, int length)
            => new LineRange(start, start + length);

        public override string ToString()
        {
            return $"[{start}, {Length}]";
        }
    }
}
