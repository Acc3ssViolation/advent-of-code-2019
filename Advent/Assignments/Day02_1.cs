using Advent.Intcode;

namespace Advent.Assignments
{
    internal class Day02_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            var memory = input[0].ExtractLongs().ToArray();

            if (!isTest)
            {
                // Patch
                memory[1] = 12;
                memory[2] = 2;
            }

            var processor = new Processor(memory);
            while (!processor.Halted)
                processor.Step();

            return memory[0].ToString();
        }
    }
}
