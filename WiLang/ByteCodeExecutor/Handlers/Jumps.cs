using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WiLang
{
    static class Jumps
    {

        public static void _ConditionalJump(bool jumpIfTrue, ref long ip, ref Instruction[] bytecode, ref WiStack<Variable> stack)
        {
            var instr = bytecode[ip];
            int? targetAddr = instr.Arg?.I;
            if (stack.Count == 0)
                throw new Exception($"Jump: stack is empty. IP: {ip}");
            var val = stack.Pop();
            bool isTrue =
                val.VarType == Types.TInteger && val.AsInt() != 0 ||
                val.VarType == Types.TFloat && val.AsFloat() != 0.0 ||
                val.VarType == Types.TString && !string.IsNullOrEmpty(val.AsString());

            if ((jumpIfTrue && isTrue) || (!jumpIfTrue && !isTrue))
            {
                if (targetAddr < 0 || targetAddr >= bytecode.Length)
                    throw new Exception($"Out of bytecode jump. IP: {ip}");
                ip = targetAddr.Value - 1;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void _MakeJump(long rf, ref long ip, ref Instruction[] bytecode, ref WiStack<Variable> stack)
        {
            if (rf < 0 || rf >= bytecode.Length)
                throw new Exception($"Out of bytecode jump. IP: {ip}");
            ip = rf - 1;
        }
    }
}
