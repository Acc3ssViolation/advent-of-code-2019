namespace Advent.Shared
{
    public struct Point
    {
        public static readonly Point North = new(0, -1);
        public static readonly Point South = new(0, 1);
        public static readonly Point East = new(1, 0);
        public static readonly Point West = new(-1, 0);
        public static readonly Point Zero = new(0, 0);

        public int x;
        public int y;

        public int Length => Math.Abs(x) + Math.Abs(y);

        public Point()
        {
            x = 0;
            y = 0;
        }

        public Point(Point point)
        {
            x = point.x;
            y = point.y;
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point a) => a;
        public static Point operator -(Point a) => new(-a.x, -a.y);
        public static Point operator +(Point a, Point b) => new(a.x + b.x, a.y + b.y);
        public static Point operator -(Point a, Point b) => new(a.x - b.x, a.y - b.y);
        public static Point operator *(int n, Point a) => new(a.x * n, a.y * n);
        public static Point operator *(Point a, int n) => new(a.x * n, a.y * n);
        public static Point operator /(Point a, int n) => new(a.x / n, a.y / n);

        public static bool operator ==(Point a, Point b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Point a, Point b) => a.x != b.x || a.y != b.y;

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public override string ToString()
        {
            return $"[{x},{y}]";
        }

        public override bool Equals(object? obj)
        {
            return obj is Point p && p == this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
