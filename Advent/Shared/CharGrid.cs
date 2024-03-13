using Advent.Shared;
using System.Text;
using System.Xml.Linq;

namespace Advent.Assignments
{
    public interface ICharGrid
    {
        int Width { get; }
        int Height { get; }
        char[] Chars { get; }
        char this[Point point] { get; set; }
    }

    public class CharGridSwapXY: ICharGrid
    {
        public int Width { get; }
        public int Height { get; }
        public char[] Chars => _data;

        public char this[Point point]
        {
            get => _data[point.y + point.x * Height];
            set => _data[point.y + point.x * Height] = value;
        }

        private readonly char[] _data;

        public CharGridSwapXY(ICharGrid grid)
        {
            Width = grid.Height;
            Height = grid.Width;
            _data = grid.Chars;
        }
    }

    public class CharGridSwapXYMirrorY : ICharGrid
    {
        public int Width { get; }
        public int Height { get; }
        public char[] Chars => _data;

        public char this[Point point]
        {
            get => _data[(Width - 1 - point.y) + point.x * Height];
            set => _data[(Width - 1 - point.y) + point.x * Height] = value;
        }

        private readonly char[] _data;

        public CharGridSwapXYMirrorY(ICharGrid grid)
        {
            Width = grid.Height;
            Height = grid.Width;
            _data = grid.Chars;
        }
    }

    public class CharGridMirrorY : ICharGrid
    {
        public int Width { get; }
        public int Height { get; }
        public char[] Chars => _data;

        public char this[Point point]
        {
            get => _data[point.x + (Height - 1 - point.y) * Width];
            set => _data[point.x + (Height - 1 - point.y) * Width] = value;
        }

        private readonly char[] _data;

        public CharGridMirrorY(ICharGrid grid)
        {
            Width = grid.Width;
            Height = grid.Height;
            _data = grid.Chars;
        }
    }

    public class CharGrid : ICharGrid
    {
        public int Width { get; }
        public int Height { get; }

        public char[] Chars => _data;

        private readonly char[] _data;

        public char this[Point point]
        {
            get => _data[point.x + point.y * Width];
            set => _data[point.x + point.y * Width] = value;
        }

        public CharGrid(CharGrid grid)
        {
            Width = grid.Width;
            Height = grid.Height;
            _data = new char[Width * Height];
            grid._data.CopyTo(_data, 0);
        }

        public CharGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new char[Width * Height];
        }

        public CharGrid(IReadOnlyList<string> strings)
        {
            Width = strings[0].Length;
            Height = strings.Count;
            _data = new char[Width * Height];
            for (var i = 0; i < strings.Count; i++)
            {
                strings[i].CopyTo(0, _data, i * Width, Width);
            }
        }

        public Span<char> GetRow(int row)
        {
            return _data.AsSpan(row * Width, Width);
        }

        public Span<char> GetCol(int col)
        {
            var result = new char[Height];
            for (var y = 0; y < Height; y++)
            {
                result[y] = _data[y * Width + col];
            }
            return result;
        }

        public Point Find(char chr)
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    if (_data[y * Width + x] == chr)
                        return new Point(x, y);
                }
            return default;
        }

        public override string ToString()
        {
            var nl = Environment.NewLine.Length;
            var sb = new char[((Width + nl) * Height)];

            for (var y = 0; y < Height; y++)
            {
                _data.AsSpan(y * Width, Width).CopyTo(sb.AsSpan(y * (Width + nl), Width));
                Environment.NewLine.CopyTo(sb.AsSpan(y * (Width + nl) + Width, nl));
            }
            return new string(sb);
        }
    }
}
