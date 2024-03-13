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
            Halt = 99,
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

            var op = (Opcode)memory[_pc];
            var arg0 = memory[_pc + 1];
            var arg1 = memory[_pc + 2];
            var arg2 = memory[_pc + 3];
            var pcInc = 4;

            switch (op)
            {
                case Opcode.Add:
                    {
                        var a = memory[arg0];
                        var b = memory[arg1];
                        memory[arg2] = a + b;
                    }
                    break;
                case Opcode.Multiply:
                    {
                        var a = memory[arg0];
                        var b = memory[arg1];
                        memory[arg2] = a * b;
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

        private static void HaltAndCatchFire()
            => throw new NotImplementedException();
    }
}
