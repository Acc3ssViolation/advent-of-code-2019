﻿using Advent.Intcode;

namespace Advent.Assignments
{
    internal class Day05_1 : IAssignment
    {
        internal class TESTProcessor : Processor
        {
            public int LastOutput { get; private set; }
            public TESTProcessor(Memory<int> memory) : base(memory)
            {
            }

            protected override int GetInput()
            {
                return 1;
            }

            protected override void SetOutput(int value)
            {
                LastOutput = value;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            if (isTest)
                return string.Empty;

            var memory = input[0].ExtractInts().ToArray();

            var processor = new TESTProcessor(memory);
            while (!processor.Halt)
                processor.Step();

            return processor.LastOutput.ToString();
        }
    }
}