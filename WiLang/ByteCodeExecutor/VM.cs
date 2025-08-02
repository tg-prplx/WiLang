using System;
using System.Collections.Generic;

namespace WiLang
{
    class VM
    {
        private Dictionary<string, Variable> vars = new();
        private Stack<Variable> stack = new();
        private long ip = 0;
        private object[] bytecode;
        private Stack<long> callStack = new();

        private int _SV(It i)
        {
            switch (i)
            {
                case It.PUSH:
                    ip++;
                    if (ip >= bytecode.Length || bytecode[ip] == null) throw new Exception($"Pushing value cannot be null or missing. IP: {ip}");
                    var v = bytecode[ip];
                    if (v is int iv)
                        stack.Push(new Variable(Types.TInteger, iv));
                    else if (v is double dv)
                        stack.Push(new Variable(Types.TFloat, dv));
                    else if (v is string sv)
                        stack.Push(new Variable(Types.TString, sv));
                    else
                        throw new Exception($"Push: unsupported literal type {v.GetType().Name}. IP: {ip}");
                    break;
                case It.POP:
                    if (stack.Count == 0) throw new Exception($"Popping stack cannot be empty. IP: {ip}");
                    stack.Pop();
                    break;
                case It.ADD:
                    BaseOPs.ArithmeticOp(Ariphmetics.AddOp, "Add", ref stack, ref ip);
                    break;
                case It.SUB:
                    BaseOPs.ArithmeticOp(Ariphmetics.SubOp, "Sub", ref stack, ref ip);
                    break;
                case It.MUL:
                    BaseOPs.ArithmeticOp(Ariphmetics.MulOp, "Mul", ref stack, ref ip);
                    break;
                case It.DIV:
                    BaseOPs.ArithmeticOp(Ariphmetics.DivOp, "Div", ref stack, ref ip);
                    break;
                case It.MOD:
                    BaseOPs.ArithmeticOp(Ariphmetics.ModOp, "Mod", ref stack, ref ip);
                    break;
                case It.SQRT:
                    BaseOPs.SqrtOp(ref stack, ref ip);
                    break;
                case It.PRINT:
                    if (stack.Count == 0) throw new Exception($"Print: stack is empty. IP: {ip}");
                    var val = stack.Peek();
                    Console.WriteLine(val.Value);
                    break;
                case It.STORE:
                    if (stack.Count < 1) throw new Exception($"Store instruction needs at least 1 value in stack. IP: {ip}");
                    ip++;
                    var valueST = stack.Pop();
                    var nameST = bytecode[ip];
                    if (nameST is string sname) vars[sname] = valueST;
                    else throw new Exception($"Store variable name must be String type. IP: {ip}");
                    break;
                case It.LOAD:
                    ip++;
                    var nameLD = bytecode[ip];
                    if (nameLD is string lname)
                    {
                        if (vars.TryGetValue(lname, out Variable varLD)) stack.Push(varLD);
                        else throw new Exception($"Cannot load variable {lname} because it doesn't exist. IP: {ip}");
                    }
                    else throw new Exception($"Load variable name must be String type. IP: {ip}");
                    break;
                case It.JUMP:
                    ip++;
                    var rf = bytecode[ip];
                    Jumps._MakeJump(rf, ref ip, ref bytecode, ref stack);
                    break;
                case It.JZ:
                    Jumps._ConditionalJump(false, ref ip, ref bytecode, ref stack);
                    break;
                case It.JNZ:
                    Jumps._ConditionalJump(true, ref ip, ref bytecode, ref stack);
                    break;
                case It.LT:
                    BaseOPs.CompareOp((a, b) => a < b, "LT", ref stack, ref ip);
                    break;
                case It.GT:
                    BaseOPs.CompareOp((a, b) => a > b, "GT", ref stack, ref ip);
                    break;
                case It.LE:
                    BaseOPs.CompareOp((a, b) => a <= b, "LE", ref stack, ref ip);
                    break;
                case It.GE:
                    BaseOPs.CompareOp((a, b) => a >= b, "GE", ref stack, ref ip);
                    break;
                case It.EQ:
                    BaseOPs.CompareOp((a, b) => a == b, "EQ", ref stack, ref ip);
                    break;
                case It.NE:
                    BaseOPs.CompareOp((a, b) => a != b, "NE", ref stack, ref ip);
                    break;
                case It.DUP:
                    if (stack.Count == 0) throw new Exception("DUP: stack is empty");
                    var item = stack.Peek();
                    stack.Push(item.Clone());
                    break;
                case It.INC:
                    BaseOPs.IncDec(stack, true);
                    break;
                case It.DEC:
                    BaseOPs.IncDec(stack, false);
                    break;
                case It.HALT:
                    ip = bytecode.Length;
                    break;
                case It.CALL:
                    ip++;
                    callStack.Push(ip + 1);
                    Jumps._MakeJump(bytecode[ip], ref ip, ref bytecode, ref stack);
                    break;
                case It.RET:
                    var rft = callStack.Pop();
                    Jumps._MakeJump(rft, ref ip, ref bytecode, ref stack);
                    break;
                case It.CASTF:
                    var castf = stack.Pop();
                    stack.Push(castf.CastToFloat());
                    break;
                case It.CASTI:
                    var casti = stack.Pop();
                    stack.Push(casti.CastToInt());
                    break;
                case It.CASTS:
                    var casts = stack.Pop();
                    stack.Push(casts.CastToString());
                    break;
                default:
                    throw new Exception($"Unknown instruction {i} at IP: {ip}");
            }
            return 0;
        }

        public void Execv(object[] bytecode)
        {
            if (bytecode == null || bytecode.Length == 0)
                throw new Exception("Bytecode is empty or null.");
            this.bytecode = bytecode;
            ip = 0;
            while (ip < bytecode.Length)
            {
                if (bytecode[ip] is not It instr)
                    throw new Exception($"Unknown instruction: {bytecode[ip]} at IP: {ip}");
                _SV(instr);
                ip++;
            }
        }
    }
}