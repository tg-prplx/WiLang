using System;
using System.Collections.Generic;

namespace WiLang
{
    class VM
    {
        private Dictionary<string, Variable> vars = new();
        private WiStack<Variable> stack = new(1024);
        private long ip = 0;
        private Instruction[] bytecode;
        private WiStack<long> callStack = new(512);

        private int _SV(Instruction instr)
        {
            switch (instr.Op)
            {
                case It.PUSH:
                    var argPush = instr.Arg ?? throw new Exception($"PUSH: argument required. IP: {ip}");
                    if (argPush.I.HasValue)
                        stack.Push(new Variable(Types.TInteger, argPush.I.Value));
                    else if (argPush.D.HasValue)
                        stack.Push(new Variable(Types.TFloat, argPush.D.Value));
                    else if (argPush.S != null)
                        stack.Push(new Variable(Types.TString, argPush.S));
                    else
                        throw new Exception($"PUSH: unsupported literal type. IP: {ip}");
                    break;
                case It.POP:
                    if (stack.Count == 0) throw new Exception($"POP: stack is empty. IP: {ip}");
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
                    if (stack.Count == 0) throw new Exception($"PRINT: stack is empty. IP: {ip}");
                    Console.WriteLine(stack.Peek().Value);
                    break;
                case It.STORE:
                    if (stack.Count < 1) throw new Exception($"STORE: stack is empty. IP: {ip}");
                    var valueST = stack.Pop();
                    var nameST = instr.Arg?.S ?? throw new Exception($"STORE: arg must be string var name. IP: {ip}");
                    vars[nameST] = valueST;
                    break;
                case It.LOAD:
                    var nameLD = instr.Arg?.S ?? throw new Exception($"LOAD: arg must be string var name. IP: {ip}");
                    if (vars.TryGetValue(nameLD, out Variable varLD))
                        stack.Push(varLD);
                    else
                        throw new Exception($"LOAD: variable '{nameLD}' not found. IP: {ip}");
                    break;
                case It.JUMP:
                    var offset = instr.Arg?.I ?? throw new Exception($"JUMP: arg required. IP: {ip}");
                    Jumps._MakeJump(offset, ref ip, ref bytecode, ref stack);
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
                    stack.Dup();
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
                    var target = instr.Arg?.I ?? throw new Exception($"CALL: arg required. IP: {ip}");
                    callStack.Push(ip + 1);
                    Jumps._MakeJump(target, ref ip, ref bytecode, ref stack);
                    break;
                case It.RET:
                    if (callStack.Count == 0) throw new Exception($"RET: call stack empty. IP: {ip}");
                    var retIP = callStack.Pop();
                    Jumps._MakeJump(retIP, ref ip, ref bytecode, ref stack);
                    break;
                case It.CASTF:
                    stack.Push(stack.Pop().CastToFloat());
                    break;
                case It.CASTI:
                    stack.Push(stack.Pop().CastToInt());
                    break;
                case It.CASTS:
                    stack.Push(stack.Pop().CastToString());
                    break;
                default:
                    throw new Exception($"Unknown instruction {instr.Op} at IP: {ip}");
            }
            return 0;
        }

        public void Execv(Instruction[] bytecode)
        {
            if (bytecode == null || bytecode.Length == 0)
                throw new Exception("Bytecode is empty or null.");
            this.bytecode = bytecode;
            ip = 0;
            while (ip < bytecode.Length)
            {
                _SV(bytecode[ip]);
                ip++;
            }
        }
    }
}
