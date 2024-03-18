using Advent.Intcode;

namespace Advent.Assignments
{
    internal class Day05_2 : IAssignment
    {
        internal class TESTProcessor : Processor
        {
            public int LastOutput { get; private set; }
            private int _input;

            public TESTProcessor(Memory<int> memory, int input) : base(memory)
            {
                _input = input;
            }

            protected override int? GetInput()
            {
                return _input;
            }

            protected override bool SetOutput(int value)
            {
                LastOutput = value;
                return true;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            var memory = input[0].ExtractInts().ToArray();

            var processor = new TESTProcessor(memory, isTest ? -21 : 5);
            while (!processor.Halted)
                processor.Step();

            return processor.LastOutput.ToString();
        }
    }
}
