namespace Advent.Assignments
{
    internal class Day01_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var sum = 0L;

            foreach (var item in input)
            {
                var mass = item.ToInt();
                var fuel = mass / 3 - 2;
                sum += fuel;
            }

            return sum.ToString();
        }
    }
}
