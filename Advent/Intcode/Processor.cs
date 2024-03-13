//#define DEBUG_ARGUMENTS

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Advent.Intcode
{
    public class Processor
    {
        public enum Flags
        {
            None = 0,
            Halt = 1,
        }

        public enum Opcode
        {
            Add = 1,
            Multiply = 2,
            Input = 3,
            Output = 4,
            JumpTrue = 5,
            JumpFalse = 6,
            LessThan = 7,
            Equals = 8,
            Halt = 99,
        }

        public enum Mode
        {
            Position = 0,
            Memory = 1,
        }

        private Memory<int> _memory;
        private int _pc;
        private Flags _flags;

        public Processor(Memory<int> memory)
        {
            _memory = memory;
        }

        public bool Halt => (_flags & Flags.Halt) != Flags.None;

        public void Step()
        {
            var memory = _memory.Span;

            var op = memory[_pc];
            Span<Mode> modes = stackalloc Mode[3];
            modes[0] = (Mode)((op / 100) % 10);
            modes[1] = (Mode)((op / 1000) % 10);
            modes[2] = (Mode)((op / 10000) % 10);
            var inst = (Opcode)(op % 100);

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            int ReadArg(Span<int> memory, Span<Mode> modes, int index)
            {
                var immediate = memory[_pc + index + 1];
                if (modes[index] == Mode.Position)
                {
#if DEBUG_ARGUMENTS
                    Logger.DebugLine($"READ[{index}] POS -> mem[{immediate}] = {memory[immediate]}");
#endif
                    return memory[immediate];
                }
#if DEBUG_ARGUMENTS
                Logger.DebugLine($"READ[{index}] IMM -> {immediate}");
#endif
                return immediate;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            void WriteArg(Span<int> memory, Span<Mode> modes, int index, int value)
            {
                var immediate = memory[_pc + index + 1];
                if (modes[index] == Mode.Position)
                {
#if DEBUG_ARGUMENTS
                    Logger.DebugLine($"WRITE[{index}] POS -> mem[{immediate}] = {value}");
#endif
                    memory[immediate] = value;
                    return;
                }

                HaltAndCatchFire();
            }

            var pcInc = 4;
#if DEBUG_ARGUMENTS
            Logger.DebugLine($"{op} -> {inst} {modes[0]} {modes[1]} {modes[2]}");
#endif
            switch (inst)
            {
                case Opcode.Add:
                    {
                        var a = ReadArg(memory, modes, 0);
                        var b = ReadArg(memory, modes, 1);
                        WriteArg(memory, modes, 2, a + b);
                    }
                    break;
                case Opcode.Multiply:
                    {
                        var a = ReadArg(memory, modes, 0);
                        var b = ReadArg(memory, modes, 1);
                        WriteArg(memory, modes, 2, a * b);
                    }
                    break;
                case Opcode.Input:
                    {
                        var input = GetInput();
                        WriteArg(memory, modes, 0, input);
                        pcInc = 2;
                    }
                    break;
                case Opcode.Output:
                    {
                        var value = ReadArg(memory, modes, 0);
                        SetOutput(value);
                        pcInc = 2;
                    }
                    break;
                case Opcode.JumpTrue:
                    {
                        var jump = ReadArg(memory, modes, 0) != 0;
                        if (jump)
                        {
                            pcInc = 0;
                            _pc = ReadArg(memory, modes, 1);
                        }
                        else
                        {
                            pcInc = 3;
                        }
                    }
                    break;
                case Opcode.JumpFalse:
                    {
                        var jump = ReadArg(memory, modes, 0) == 0;
                        if (jump)
                        {
                            pcInc = 0;
                            _pc = ReadArg(memory, modes, 1);
                        }
                        else
                        {
                            pcInc = 3;
                        }
                    }
                    break;
                case Opcode.LessThan:
                    {
                        var a = ReadArg(memory, modes, 0);
                        var b = ReadArg(memory, modes, 1);
                        if (a < b)
                            WriteArg(memory, modes, 2, 1);
                        else
                            WriteArg(memory, modes, 2, 0);
                    }
                    break;
                case Opcode.Equals:
                    {
                        var a = ReadArg(memory, modes, 0);
                        var b = ReadArg(memory, modes, 1);
                        if (a == b)
                            WriteArg(memory, modes, 2, 1);
                        else
                            WriteArg(memory, modes, 2, 0);
                    }
                    break;
                case Opcode.Halt:
                    _flags |= Flags.Halt;
                    pcInc = 1;
                    break;
                default:
                    HaltAndCatchFire();
                    break;
            }

            _pc += pcInc;
        }

        public void StepChecked()
        {
            checked
            {
                Step();
            }
        }

        public void Jump(int address)
            => _pc = address;

        public void Reset(Memory<int> memory)
        {
            _pc = 0;
            _memory = memory;
            _flags = Flags.None;
        }

        protected virtual int GetInput()
            => HaltAndCatchFire<int>();
        protected virtual void SetOutput(int value)
            => HaltAndCatchFire<int>();

        [DoesNotReturn]
        private static T HaltAndCatchFire<T>()
            => throw new NotImplementedException();

        [DoesNotReturn]
        private static void HaltAndCatchFire()
            => throw new NotImplementedException();
    }
}
