using System.Runtime.CompilerServices;

namespace WiLang
{
    public struct Instruction
    {
        public It Op;
        public WiValue? Arg;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Instruction(It op, WiValue? arg = null) { Op = op; Arg = arg; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Instruction(It op, int arg) : this(op, new WiNumberValue(new WiNumber(arg))) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Instruction(It op, double arg) : this(op, new WiFloatValue(new WiFloat((float)arg))) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Instruction(It op, string arg) : this(op, new WiStringValue(new WiString(arg))) { }
    }
}
