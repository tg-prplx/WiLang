using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WiLang
{
    class VM
    {
        private Dictionary<string, Variable> vars = new();
        private WiStack<Variable> stack = new(1024);
        private long ip = 0;
        private Instruction[] bytecode;
        private WiStack<long> callStack = new(512);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CompareAndPush(ref WiStack<Variable> stack, Func<Variable, Variable, bool> cmp)
        {
            var b = stack.Pop();
            var a = stack.Pop();
            stack.Push(new Variable(Types.TInteger, cmp(a, b) ? 1 : 0));
        }

        private int _SV(Instruction instr)
        {
            switch (instr.Op)
            {
                case It.PUSH:
                    {
                        var argPush = instr.Arg ?? throw new Exception($"PUSH: argument required. IP: {ip}");
                        switch (argPush)
                        {
                            case WiNumberValue nm:
                                stack.Push(new Variable(Types.TInteger, nm.Value.Value));
                                break;
                            case WiFloatValue flt:
                                stack.Push(new Variable(Types.TFloat, flt.Value.Value));
                                break;
                            case WiStringValue str:
                                stack.Push(new Variable(Types.TString, str.Value.Value));
                                break;
                            default:
                                throw new Exception($"PUSH: unsupported literal type. IP: {ip}");
                        }
                        break;
                    }
                case It.POP:
                    if (stack.Count == 0) throw new Exception($"POP: stack is empty. IP: {ip}");
                    stack.Pop();
                    break;
                case It.ADD:
                    if (stack.Count < 2)
                        throw new Exception($"ADD: stack has less than 2 values! IP: {ip}");
                    stack.Push(stack.Pop() + stack.Pop());
                    break;
                case It.SUB:
                    if (stack.Count < 2)
                        throw new Exception($"SUB: stack has less than 2 values! IP: {ip}");
                    {
                        var b = stack.Pop();
                        var a = stack.Pop();
                        stack.Push(a - b);
                    }
                    break;
                case It.MUL:
                    if (stack.Count < 2)
                        throw new Exception($"MUL: stack has less than 2 values! IP: {ip}");
                    stack.Push(stack.Pop() * stack.Pop());
                    break;
                case It.DIV:
                    if (stack.Count < 2)
                        throw new Exception($"DIV: stack has less than 2 values! IP: {ip}");
                    {
                        var b = stack.Pop();
                        var a = stack.Pop();
                        stack.Push(a / b);
                    }
                    break;
                case It.MOD:
                    if (stack.Count < 2)
                        throw new Exception($"MOD: stack has less than 2 values! IP: {ip}");
                    {
                        var b = stack.Pop();
                        var a = stack.Pop();
                        stack.Push(a % b);
                    }
                    break;
                case It.SQRT:
                    {
                        var val = stack.Pop();
                        if (val.VarType == Types.TInteger)
                            stack.Push(new Variable(Types.TFloat, Math.Sqrt(val.AsInt())));
                        else if (val.VarType == Types.TFloat)
                            stack.Push(new Variable(Types.TFloat, Math.Sqrt(val.AsFloat())));
                        else
                            throw new Exception($"Sqrt: unsupported type {val.VarType}. IP: {ip}");
                    }
                    break;
                case It.PRINT:
                    if (stack.Count == 0) throw new Exception($"PRINT: stack is empty. IP: {ip}");
                    Console.WriteLine(stack.Peek().Value);
                    break;
                case It.INPUT:
                    stack.Push(new Variable(Types.TString, Console.ReadLine() ?? ""));
                    break;
                case It.STORE:
                    if (stack.Count < 1) throw new Exception($"STORE: stack is empty. IP: {ip}");
                    var valueST = stack.Pop();
                    var nameST = instr.Arg is WiStringValue s ? s.Value.Value : null;
                    if (nameST == null)
                        throw new Exception($"STORE: arg must be string var name. IP: {ip}");
                    vars[nameST] = valueST;
                    break;
                case It.LOAD:
                    var nameLD = instr.Arg is WiStringValue ws ? ws.Value.Value : null;
                    if (nameLD == null)
                        throw new Exception($"LOAD: arg must be string var name. IP: {ip}");
                    if (vars.TryGetValue(nameLD, out Variable varLD))
                        stack.Push(varLD);
                    else
                        throw new Exception($"LOAD: variable '{nameLD}' not found. IP: {ip}");
                    break;
                case It.JUMP:
                    var offset = instr.Arg is WiNumberValue num ? num.Value.Value : (int?)null;
                    if (offset == null)
                        throw new Exception($"JUMP: arg required. IP: {ip}");
                    Jumps._MakeJump(offset.Value, ref ip, ref bytecode, ref stack);
                    break;
                case It.JZ:
                    Jumps._ConditionalJump(false, ref ip, ref bytecode, ref stack);
                    break;

                case It.LT: CompareAndPush(ref stack, (a, b) => a < b); break;
                case It.GT: CompareAndPush(ref stack, (a, b) => a > b); break;
                case It.LE: CompareAndPush(ref stack, (a, b) => a <= b); break;
                case It.GE: CompareAndPush(ref stack, (a, b) => a >= b); break;
                case It.EQ: CompareAndPush(ref stack, (a, b) => a == b); break;
                case It.NE: CompareAndPush(ref stack, (a, b) => a != b); break;

                case It.DUP:
                    stack.Dup();
                    break;
                case It.INC:
                    {
                        var val = stack.Pop();
                        if (val.VarType == Types.TInteger)
                            stack.Push(new Variable(Types.TInteger, val.AsInt() + 1));
                        else if (val.VarType == Types.TFloat)
                            stack.Push(new Variable(Types.TFloat, val.AsFloat() + 1.0));
                        else
                            throw new Exception($"INC: unsupported type {val.VarType}");
                    }
                    break;
                case It.DEC:
                    {
                        var val = stack.Pop();
                        if (val.VarType == Types.TInteger)
                            stack.Push(new Variable(Types.TInteger, val.AsInt() - 1));
                        else if (val.VarType == Types.TFloat)
                            stack.Push(new Variable(Types.TFloat, val.AsFloat() - 1.0));
                        else
                            throw new Exception($"DEC: unsupported type {val.VarType}");
                    }
                    break;
                case It.HALT:
                    ip = bytecode.Length;
                    break;
                case It.CALL:
                    var offsetС = instr.Arg is WiNumberValue numС ? numС.Value.Value : (int?)null;
                    if (offsetС == null)
                        throw new Exception($"JUMP: arg required. IP: {ip}");
                    callStack.Push(ip + 1);
                    Jumps._MakeJump(offsetС.Value, ref ip, ref bytecode, ref stack);
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
