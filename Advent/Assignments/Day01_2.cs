namespace Advent.Assignments
{
    internal class Day01_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var sum = 0L;

            foreach (var item in input)
            {
                var mass = item.ToInt();
                while (mass > 0)
                {
                    mass = mass / 3 - 2;
                    if (mass > 0)
                        sum += mass;
                }
            }

            return sum.ToString();
        }
    }
}
