using Advent.Intcode;

namespace Advent.Assignments
{
    internal class Day05_1 : IAssignment
    {
        internal class TESTProcessor : Processor
        {
            public int LastOutput { get; private set; }
            public TESTProcessor(Memory<long> memory) : base(memory)
            {
            }

            protected override long? GetInput()
            {
                return 1;
            }

            protected override bool SetOutput(long value)
            {
                LastOutput = (int)value;
                return true;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            if (isTest)
                return string.Empty;

            var memory = input[0].ExtractLongs().ToArray();

            var processor = new TESTProcessor(memory);
            while (!processor.Halted)
                processor.Step();

            return processor.LastOutput.ToString();
        }
    }
}
