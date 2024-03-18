//#define DEBUG_ARGUMENTS

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Advent.Intcode
{
    public class Processor
    {
        [Flags]
        public enum Flags
        {
            None = 0,
            Halt = 1 << 0,
            InputBlocked = 1 << 2,
            OutputBlocked = 1 << 3,
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
            Relative = 2,
        }

        private Memory<long> _memory;
        private int _pc;
        private Flags _flags;

        public Processor(Memory<long> memory)
        {
            _memory = memory;
        }

        public bool Halted => (_flags & Flags.Halt) != Flags.None;

        public bool IoBlocked => (_flags & (Flags.InputBlocked | Flags.OutputBlocked)) != Flags.None;

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
            long ReadArg(Span<long> memory, Span<Mode> modes, int index)
            {
                var immediate = (int)memory[_pc + index + 1];
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
            void WriteArg(Span<long> memory, Span<Mode> modes, int index, long value)
            {
                var immediate = (int)memory[_pc + index + 1];
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
                        if (input.HasValue)
                        {
                            _flags &= ~Flags.InputBlocked;
                            WriteArg(memory, modes, 0, input.Value);
                            pcInc = 2;
                        }
                        else
                        {
                            _flags |= Flags.InputBlocked;
                            pcInc = 0;
                        }
                    }
                    break;
                case Opcode.Output:
                    {
                        var value = ReadArg(memory, modes, 0);
                        if (SetOutput(value))
                        {
                            _flags &= ~Flags.OutputBlocked;
                            pcInc = 2;
                        }
                        else
                        {
                            _flags |= Flags.OutputBlocked;
                            pcInc = 0;
                        }
                    }
                    break;
                case Opcode.JumpTrue:
                    {
                        var jump = ReadArg(memory, modes, 0) != 0;
                        if (jump)
                        {
                            pcInc = 0;
                            _pc = (int)ReadArg(memory, modes, 1);
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
                            _pc = (int)ReadArg(memory, modes, 1);
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

        public virtual void Reset(Memory<long> memory)
        {
            _pc = 0;
            _memory = memory;
            _flags = Flags.None;
        }

        protected virtual long? GetInput()
            => HaltAndCatchFire<long>();
        protected virtual bool SetOutput(long value)
            => HaltAndCatchFire<bool>();

        public void RunUntilHalt()
        {
            while (!Halted)
                Step();
        }

        public void RunUntilHaltOrBlocked()
        {
            while (!Halted)
            {
                Step();
                if (IoBlocked)
                    return;
            }
        }

        [DoesNotReturn]
        private static T HaltAndCatchFire<T>()
            => throw new NotImplementedException();

        [DoesNotReturn]
        private static void HaltAndCatchFire()
            => throw new NotImplementedException();
    }
}
