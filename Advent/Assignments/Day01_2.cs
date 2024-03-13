namespace Advent.Assignments
{
    internal class Day01_2 : IAssignment
    {
        private static readonly string[] DigitStrings = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        private static readonly char[] Digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public string Run(IReadOnlyList<string> input)
        {
            var sum = 0;

            foreach (var item in input)
            {
                var v0 = FindDigit(item);
                var v1 = FindLastDigit(item);

                sum += 10 * v0 + v1;
            }

            return sum.ToString();
        }

        private static int FindLastDigit(string input)
        {
            var highestIndex = -1;
            var digit = 0;
            for (var i = 0; i < DigitStrings.Length; i++)
            {
                var digitIndex0 = input.LastIndexOf(Digits[i]);
                var digitIndex1 = input.LastIndexOf(DigitStrings[i]);
                var index = digitIndex0 > digitIndex1 ? digitIndex0 : digitIndex1;
                if (index > highestIndex)
                {
                    highestIndex = index;
                    digit = i;
                }
            }

            return digit;
        }

        private static int FindDigit(string input)
        {
            var lowestIndex = int.MaxValue;
            var digit = 0;
            for (var i = 0; i < DigitStrings.Length; i++)
            {
                var digitIndex0 = input.IndexOf(Digits[i]);
                if (digitIndex0 < 0)
                    digitIndex0 = int.MaxValue;
                var digitIndex1 = input.IndexOf(DigitStrings[i]);
                if (digitIndex1 < 0)
                    digitIndex1 = int.MaxValue;
                var index = digitIndex0 < digitIndex1 ? digitIndex0 : digitIndex1;
                if (index < lowestIndex)
                {
                    lowestIndex = index;
                    digit = i;
                }
            }

            return digit;
        }
    }
}
