namespace Advent.Assignments
{
    internal class Day01_1 : IAssignment
    {
        private static readonly char[] Digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public string Run(IReadOnlyList<string> input)
        {
            var sum = 0;

            foreach (var item in input)
            {
                var d0 = item.IndexOfAny(Digits);
                var d1 = item.LastIndexOfAny(Digits);

                var v0 = item[d0] - '0';
                var v1 = item[d1] - '0';

                sum += 10 * v0 + v1;
            }

            return sum.ToString();
        }
    }
}
