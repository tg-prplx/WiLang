using System.Runtime.CompilerServices;

namespace WiLang
{
    public struct Instruction
    {
        public It Op;
        public Operand? Arg;
        public Instruction(It op, Operand? arg = null) { Op = op; Arg = arg; }
    }
    public struct Operand
    {
        public int? I;
        public double? D;
        public string? S;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(int i) { I = i; D = null; S = null; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(double d) { I = null; D = d; S = null; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(string s) { I = null; D = null; S = s; }
    }
}
