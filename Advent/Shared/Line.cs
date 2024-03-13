namespace Advent.Shared
{
    internal struct Line
    {
        public Point start;
        public Point end;

        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public override string ToString()
        {
            return $"({start}, {end})";
        }
    }
}
