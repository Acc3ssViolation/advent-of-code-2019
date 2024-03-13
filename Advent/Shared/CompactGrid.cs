namespace Advent.Shared
{
    internal class CompactGrid
    {
        public int Width { get; }
        public int Height { get; }

        private byte[] _data;

        public byte this[Point point]
        {
            get => _data[point.x + point.y * Width];
            set => _data[point.x + point.y * Width] = value;
        }

        public CompactGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new byte[Width * Height];
        }

        public override string ToString()
        {
            var nl = Environment.NewLine.Length;
            var sb = new char[((Width + nl) * Height)];

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    sb[y * (Width + nl) + x] = (char)(_data[y * Width + x] + '0');
                }
                Environment.NewLine.CopyTo(sb.AsSpan(y * (Width + nl) + Width, nl));
            }
            return new string(sb);
        }
    }
}
