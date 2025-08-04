using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WiLang
{
    static class Jumps
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void _ConditionalJump(bool jumpIfTrue, ref long ip, ref Instruction[] bytecode, ref WiStack<Variable> stack)
        {
            var instr = bytecode[ip];
            if (instr.Arg is not WiNumberValue num)
                throw new Exception($"Jump: bad or missing argument. IP: {ip}");
            int targetAddr = num.Value.Value ?? 0;
            if (stack.Count == 0) throw new Exception($"Jump: stack is empty. IP: {ip}");
            var val = stack.Pop();
            bool isTrue = val.AsBool();
            if ((jumpIfTrue && isTrue) || (!jumpIfTrue && !isTrue))
            {
                if (targetAddr < 0 || targetAddr >= bytecode.Length)
                    throw new Exception($"Out of bytecode jump. IP: {ip}");
                ip = targetAddr - 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void _MakeJump(long target, ref long ip, ref Instruction[] bytecode, ref WiStack<Variable> stack)
        {
            if (target < 0 || target >= bytecode.Length)
                throw new Exception($"Out of bytecode jump. IP: {ip}");
            ip = target - 1;
        }
    }
}

