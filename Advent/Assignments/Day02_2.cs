using Advent.Intcode;

namespace Advent.Assignments
{
    internal class Day02_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            if (isTest)
                return string.Empty;

            var defaultMemory = input[0].ExtractLongs().ToArray();
            var memory = defaultMemory.ToArray();
            var processor = new Processor(memory);

            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    memory[1] = noun;
                    memory[2] = verb;
                    processor.Reset(memory);
                    while (!processor.Halted)
                        processor.Step();
                    var result = memory[0];
                    if (result == 19690720)
                        return (100 * noun + verb).ToString();

                    Array.Copy(defaultMemory, memory, defaultMemory.Length);
                }
            }

            return "FAIL";
        }
    }
}
