namespace Advent.Shared
{
    public struct RealPoint
    {
        public static readonly RealPoint North = new(0, -1);
        public static readonly RealPoint South = new(0, 1);
        public static readonly RealPoint East = new(1, 0);
        public static readonly RealPoint West = new(-1, 0);
        public static readonly RealPoint Zero = new(0, 0);

        public double x;
        public double y;

        public RealPoint()
        {
            x = 0;
            y = 0;
        }

        public RealPoint(RealPoint point)
        {
            x = point.x;
            y = point.y;
        }

        public RealPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public RealPoint(Point point)
        {
            this.x = point.x;
            this.y = point.y;
        }

        public static RealPoint operator +(RealPoint a) => a;
        public static RealPoint operator -(RealPoint a) => new(-a.x, -a.y);
        public static RealPoint operator +(RealPoint a, RealPoint b) => new(a.x + b.x, a.y + b.y);
        public static RealPoint operator -(RealPoint a, RealPoint b) => new(a.x - b.x, a.y - b.y);
        public static RealPoint operator *(double n, RealPoint a) => new(a.x * n, a.y * n);
        public static RealPoint operator *(RealPoint a, double n) => new(a.x * n, a.y * n);
        public static RealPoint operator /(RealPoint a, double n) => new(a.x / n, a.y / n);

        public static bool operator ==(RealPoint a, RealPoint b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(RealPoint a, RealPoint b) => a.x != b.x || a.y != b.y;

        public RealPoint Normalize()
        {
            var factor = 1 / Math.Sqrt(x * x + y * y);
            return new RealPoint(x * factor, y * factor);
        }

        public void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }

        public override string ToString()
        {
            return $"[{x}; {y}]";
        }

        public override bool Equals(object? obj)
        {
            return obj is RealPoint p && p == this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
