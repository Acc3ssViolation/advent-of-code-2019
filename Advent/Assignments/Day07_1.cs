using Advent.Intcode;
using Advent.Shared;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day07_1 : IAssignment
    {
        internal class AmpProcessor : Processor
        {
            public int LastOutput { get; private set; }
            public int Phase { get; set; }
            public int Input { get; set; }

            private bool _inputFlag;

            public AmpProcessor(Memory<long> memory) : base(memory)
            {
            }

            protected override long? GetInput()
            {
                if (_inputFlag)
                    return Input;

                _inputFlag = true;
                return Phase;
            }

            protected override bool SetOutput(long value)
            {
                LastOutput = (int)value;
                return true;
            }

            public override void Reset(Memory<long> memory)
            {
                base.Reset(memory);
                LastOutput = 0;
                _inputFlag = false;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            int CalculateOutput(AmpProcessor processor, IReadOnlyList<int> phase, long[] memory, long[] origMemory)
            {
                processor.Reset(memory);

                for (var i = 0; i < 5; i++)
                {
                    Array.Copy(origMemory, memory, origMemory.Length);
                    var lastOutput = processor.LastOutput;
                    processor.Reset(memory);
                    processor.Input = lastOutput;
                    processor.Phase = phase[i];
                    processor.RunUntilHalt();
                }
                return processor.LastOutput;
            }

            var memory = input[0].ExtractLongs().ToArray();
            var origMemory = memory.ToArray();
            var processor = new AmpProcessor(memory);

            var phases = new int[] { 0, 1, 2, 3, 4 };
            var phaseConfigurations = phases.GetPermutations();
            IEnumerable<int>? highestConfig = null;
            var highestValue = 0;
            foreach (var config in phaseConfigurations)
            {
                var value = CalculateOutput(processor, config, memory, origMemory);
                if (value >  highestValue || highestConfig == null)
                {
                    highestConfig = config;
                    highestValue = value;
                }
            }
            return highestValue.ToString();
        }
    }
}
