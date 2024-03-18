using Advent.Intcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day09_2 : IAssignment
    {
        class BoostProcessor : Processor
        {
            public BoostProcessor(Memory<long> memory) : base(memory)
            {
            }

            protected override long? GetInput()
            {
                return 2;
            }

            protected override bool SetOutput(long value)
            {
                Logger.DebugLine($"-> {value}");
                return true;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            var memory = input[0].ExtractLongs().ToArray();
            var boost = new BoostProcessor(memory);
            boost.RunUntilHaltOrBlocked();
            return "";
        }
    }
}
