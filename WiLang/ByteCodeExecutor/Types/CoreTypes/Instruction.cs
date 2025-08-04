using System.Runtime.CompilerServices;

namespace WiLang
{
    public struct Instruction
    {
        public It Op;
        public Operand? Arg;
        public Instruction(It op, Operand? arg = null) { Op = op; Arg = arg; }
    }
}
