using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    static class Jumps
    {
        public static void _ConditionalJump(bool jumpIfTrue, ref long ip, ref object[] bytecode, ref Stack<Variable> stack)
        {
            ip++;
            var addr = bytecode[ip];
            if (!(addr is int targetAddr)) throw new Exception($"Jump arg must be integer. IP: {ip}");
            if (stack.Count == 0) throw new Exception($"Jump: stack is empty. IP: {ip}");
            var val = stack.Pop();
            bool isTrue =
                val.VarType == Types.TInteger && val.AsInt() != 0 ||
                val.VarType == Types.TFloat && val.AsFloat() != 0.0 ||
                val.VarType == Types.TString && !string.IsNullOrEmpty(val.AsString());
            if (jumpIfTrue && isTrue || !jumpIfTrue && !isTrue)
            {
                if (targetAddr < 0 || targetAddr >= bytecode.Length) throw new Exception($"Out of bytecode jump. IP: {ip}");
                if (!(bytecode[targetAddr] is It)) throw new Exception($"Jump target must be instruction. IP: {ip}");
                ip = targetAddr - 1;
            }
        }

        public static void _MakeJump(object rf, ref long ip, ref object[] bytecode, ref Stack<Variable> stack)
        {
            if (!(rf is int jmp)) throw new Exception($"Jump arg must be integer. IP: {ip}");
            if (jmp < 0 || jmp >= bytecode.Length) throw new Exception($"Out of bytecode jump. IP: {ip}");
            if (!(bytecode[jmp] is It)) throw new Exception($"Jump target must be instruction. IP: {ip}");
            ip = jmp - 1;
        }
    }
}
