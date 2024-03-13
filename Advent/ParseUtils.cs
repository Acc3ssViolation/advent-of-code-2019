using System.Globalization;
using System.Text.RegularExpressions;

namespace Advent
{
    internal static class ParseUtils
    {
        public static long ToLongIgnoreWhitespace(this ReadOnlySpan<char> str)
        {
            var num = 0L;
            foreach (var chr in str)
            {
                if (!char.IsNumber(chr))
                {
                    continue;
                }
                num *= 10;
                num += chr - '0';
            }
            return num;
        }

        public static int ToHexInt(this string str)
        {
            bool negative = false;
            var num = 0;
            var index = 0;
            if (str[index] == '-')
            {
                negative = true;
                index++;
            }
            while (index < str.Length)
            {
                var chr = str[index];
                if (char.IsAsciiDigit(chr))
                {
                    num *= 16;
                    num += chr - '0';
                }
                else if (char.IsBetween(chr, 'a', 'f'))
                {
                    num *= 16;
                    num += chr - 'a' + 10;
                }
                else
                {
                    break;
                }

                index++;
            }
            return negative ? -num : num;
        }

        public static int ToInt(this string str)
        {
            bool negative = false;
            var num = 0;
            var index = 0;
            if (str[index] == '-')
            {
                negative = true;
                index++;
            }
            while (index < str.Length)
            {
                var chr = str[index];
                if (!char.IsNumber(chr))
                {
                    break;
                }
                num *= 10;
                num += chr - '0';
                index++;
            }
            return negative ? -num : num;
        }

        public static List<long> ExtractLongs(this string str)
        {
            const int FlagNegative = 1;
            const int FlagPush = 2;
            var result = new List<long>(str.Length / 4);
            var temp = 0L;
            var flags = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var chr = str[i];
                var digit = chr - '0';
                if (digit >= 0 && digit <= 9)
                {
                    temp *= 10;
                    temp += digit;
                    flags |= FlagPush;
                }
                else if (chr == '-')
                {
                    flags |= FlagNegative;
                }
                else
                {
                    if ((flags & FlagNegative) != 0)
                        result.Add(-temp);
                    else
                        result.Add(temp);
                    temp = 0;
                    flags = 0;
                }
            }
            if ((flags & FlagPush) != 0)
            {
                if ((flags & FlagNegative) != 0)
                    result.Add(-temp);
                else
                    result.Add(temp);
            }
            return result;
        }

        public static List<int> ExtractInts(this string str)
        {
            const int FlagNegative = 1;
            const int FlagPush = 2;
            var result = new List<int>(str.Length / 4);
            var temp = 0;
            var flags = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var chr = str[i];
                var digit = chr - '0';
                if (digit >= 0 && digit <= 9) 
                {
                    temp *= 10;
                    temp += digit;
                    flags |= FlagPush;
                }
                else if (chr == '-')
                {
                    flags |= FlagNegative;
                }
                else if ((flags & FlagPush) != 0)
                {
                    if ((flags & FlagNegative) != 0)
                        result.Add(-temp);
                    else
                        result.Add(temp);
                    temp = 0;
                    flags = 0;
                }
            }
            if ((flags & FlagPush) != 0)
            {
                if ((flags & FlagNegative) != 0)
                    result.Add(-temp);
                else
                    result.Add(temp);
            }
            return result;
        }

        public static int ParseIntPositive(string str, ref int index)
        {
            var num = 0;
            while (index < str.Length)
            {
                var chr = str[index];
                if (!char.IsNumber(chr))
                {
                    break;
                }
                num *= 10;
                num += chr - '0';
                index++;
            }
            return num;
        }

        public static int ParseInt(string str, ref int index)
        {
            bool negative = false;
            var num = 0;
            while (index < str.Length && !char.IsNumber(str[index]) && str[index] != '-')
            {
                index++;
            }
            if (str[index] == '-')
            {
                negative = true;
                index++;
            }
            while (index < str.Length)
            {
                var chr = str[index];
                if (!char.IsNumber(chr))
                {
                    break;
                }
                num *= 10;
                num += chr - '0';
                index++;
            }
            return negative ? -num : num;
        }
    }
}
