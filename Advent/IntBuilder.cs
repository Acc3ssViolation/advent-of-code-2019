namespace Advent
{
    internal class IntBuilder
    {
        private int _value;
        private int _length;

        public bool PushChar(char c)
        {
            if (c >= '0' && c <= '9')
            {
                _value *= 10;
                _value += c - '0';
                _length++;
                return true;
            }
            return false;
        }

        public int ToInt()
        {
            return _value;
        }

        public int Length => _length;

        public void Clear()
        {
            _value = 0;
            _length = 0;
        }
    }
}
