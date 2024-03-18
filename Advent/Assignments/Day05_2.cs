using Advent.Intcode;

namespace Advent.Assignments
{
    internal class Day05_2 : IAssignment
    {
        internal class TESTProcessor : Processor
        {
            public int LastOutput { get; private set; }
            private int _input;

            public TESTProcessor(Memory<long> memory, int input) : base(memory)
            {
                _input = input;
            }

            protected override long? GetInput()
            {
                return _input;
            }

            protected override bool SetOutput(long value)
            {
                LastOutput = (int)value;
                return true;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            var memory = input[0].ExtractLongs().ToArray();

            var processor = new TESTProcessor(memory, isTest ? -21 : 5);
            while (!processor.Halted)
                processor.Step();

            return processor.LastOutput.ToString();
        }
    }
}
