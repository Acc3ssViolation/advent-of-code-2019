//#define LOG_IO
using Advent.Intcode;
using Advent.Shared;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day07_2 : IAssignment
    {
        public string TestFile => "test-day07_2.txt";

        internal class AmpProcessor : Processor
        {
            private IoBuffer _input;
            private IoBuffer _output;

            public AmpProcessor(Memory<int> memory, IoBuffer input, IoBuffer output) : base(memory)
            {
                _input = input;
                _output = output;
            }

            protected override int? GetInput()
            {
                if (_input.Read(out var value))
                    return value;

                return null;
            }

            protected override bool SetOutput(int value)
            {
                return _output.Write(value);
            }

            public override void Reset(Memory<int> memory)
            {
                base.Reset(memory);
            }
        }

        public class IoBuffer
        {
            public bool HasValue { get; private set; }
            private int _value;
            private readonly string _name;

            public IoBuffer(string name)
            {
                _name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public bool Write(int value)
            {
                if (HasValue)
                {
#if LOG_IO
                    Logger.DebugLine($"IO [{_name}] WRITE BLOCKED");
#endif
                    return false;
                }
#if LOG_IO
                Logger.DebugLine($"IO [{_name}] WRITE {value}");
#endif
                HasValue = true;
                _value = value;
                return true;
            }

            public bool Read(out int value)
            {
                if (HasValue)
                {
                    value = _value;
                    HasValue = false;
#if LOG_IO
                    Logger.DebugLine($"IO [{_name}] READ {value}");
#endif
                    return true;
                }

                value = 0;
#if LOG_IO
                Logger.DebugLine($"IO [{_name}] READ BLOCKED");
#endif
                return false;
            }

            public void Reset()
            {
                _value = default;
                HasValue = false;
            }
        }

        public string Run(IReadOnlyList<string> input, bool isTest)
        {
            var origMemory = input[0].ExtractInts().ToArray();

            var bufInOut = new IoBuffer("IN/OUT");
            var bufAToB = new IoBuffer("A->B");
            var bufBToC = new IoBuffer("B->C");
            var bufCToD = new IoBuffer("C->D");
            var bufDToE = new IoBuffer("D->E");

            var a = new AmpProcessor(origMemory.ToArray(), bufInOut, bufAToB);
            var b = new AmpProcessor(origMemory.ToArray(), bufAToB, bufBToC);
            var c = new AmpProcessor(origMemory.ToArray(), bufBToC, bufCToD);
            var d = new AmpProcessor(origMemory.ToArray(), bufCToD, bufDToE);
            var e = new AmpProcessor(origMemory.ToArray(), bufDToE, bufInOut);

            IEnumerable<int>? highestConfig = null;
            var highestValue = 0;

            var phases = new int[] { 5, 6, 7, 8, 9 };
            var phaseConfigurations = phases.GetPermutations();
            foreach (var config in phaseConfigurations)
            {
#if LOG_IO
                Logger.DebugLine($"===========================================");
                Logger.DebugLine($"{string.Join(',', config)}");
                Logger.DebugLine($"===========================================");
#endif
                var allHalted = true;
                var amps = new AmpProcessor[] { a, b, c, d, e };
                var bufs = new IoBuffer[] { bufInOut, bufAToB, bufBToC, bufCToD, bufDToE };

                // Reset the processors and the buffers
                for (var i = 0; i < amps.Length; i++)
                {
                    amps[i].Reset(origMemory.ToArray());
                    bufs[i].Reset();
                }

                // Load each processor with its phase input
                for (var i = 0; i < amps.Length; i++)
                {
                    bufs[i].Write(config[i]);
                    amps[i].RunUntilHaltOrBlocked();
                }

                // Load the initial 0 value into the in/out buffer
                bufs[0].Write(0);

                // Run until everyone is stopped
                do
                {
                    allHalted = true;
                    for (var ampIndex = 0; ampIndex < amps.Length; ampIndex++)
                    {
                        amps[ampIndex].RunUntilHaltOrBlocked();
                        if (!amps[ampIndex].Halted)
                            allHalted = false;
                    }
                } while (!allHalted);

                Debug.Assert(bufInOut.HasValue);
                bufInOut.Read(out var value);
                if (value > highestValue || highestConfig == null)
                {
                    highestConfig = config;
                    highestValue = value;
                }
            }
            return highestValue.ToString();
        }
    }
}
