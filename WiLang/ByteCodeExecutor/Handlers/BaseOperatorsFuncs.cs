using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace WiLang
{
    static class BaseOPs
    {
        public static void ArithmeticOp(Func<Variable, Variable, Variable> op, string opname, ref WiStack<Variable> stack, ref long ip)
        {
            if (stack.Count < 2)
                throw new Exception($"{opname}: needs at least 2 values in stack. IP: {ip}");
            var b = stack.Pop();
            var a = stack.Pop();
            if (a.VarType != b.VarType)
                throw new Exception($"{opname}: values of different types ({a.VarType} vs {b.VarType}). IP: {ip}");
            if (a.VarType == Types.TInteger || a.VarType == Types.TFloat)
            {
                if ((opname == "Div" || opname == "Mod") &&
                    (a.VarType == Types.TInteger && b.AsInt() == 0 ||
                     a.VarType == Types.TFloat && b.AsFloat() == 0.0))
                    throw new DivideByZeroException($"{opname}: divide by zero. IP: {ip}");

                stack.Push(op(a, b));
            }
            else if (a.VarType == Types.TString && opname == "Add")
            {
                stack.Push(op(a, b));
            }
            else
            {
                throw new Exception($"{opname}: unsupported type {a.VarType}. IP: {ip}");
            }
        }

        public static void CompareOp(Func<dynamic, dynamic, bool> cmp, string opname, ref WiStack<Variable> stack, ref long ip)
        {
            if (stack.Count < 2)
                throw new Exception($"{opname}: needs at least 2 values in stack. IP: {ip}");
            var b = stack.Pop();
            var a = stack.Pop();

            if (a.VarType != b.VarType)
                throw new Exception($"{opname}: values of different types ({a.VarType} vs {b.VarType}). IP: {ip}");

            bool result = false;

            if (a.VarType == Types.TInteger)
                result = cmp(a.AsInt(), b.AsInt());
            else if (a.VarType == Types.TFloat)
                result = cmp(a.AsFloat(), b.AsFloat());
            else if (a.VarType == Types.TString)
                result = cmp(a.AsString(), b.AsString());
            else
                throw new Exception($"{opname}: unsupported type {a.VarType}. IP: {ip}");

            stack.Push(new Variable(Types.TInteger, result ? 1 : 0));
        }

       public static void SqrtOp(ref WiStack<Variable> stack, ref long ip)
        {
            if (stack.Count < 1)
                throw new Exception($"Sqrt: stack is empty. IP: {ip}");
            var val = stack.Pop();
            if (val.VarType == Types.TInteger)
                stack.Push(new Variable(Types.TFloat, Math.Sqrt(val.AsInt())));
            else if (val.VarType == Types.TFloat)
                stack.Push(new Variable(Types.TFloat, Math.Sqrt(val.AsFloat())));
            else
                throw new Exception($"Sqrt: unsupported type {val.VarType}. IP: {ip}");
        }

        public static void IncDec(WiStack<Variable> stack, bool isInc)
        {
            if (stack.Count == 0) throw new Exception((isInc ? "INC" : "DEC") + ": stack is empty");
            var val = stack.Pop();
            switch (val.VarType)
            {
                case Types.TInteger:
                    stack.Push(new Variable(Types.TInteger, val.AsInt() + (isInc ? 1 : -1)));
                    break;
                case Types.TFloat:
                    stack.Push(new Variable(Types.TFloat, val.AsFloat() + (isInc ? 1.0 : -1.0)));
                    break;
                default:
                    throw new Exception((isInc ? "INC" : "DEC") + ": unsupported type " + val.VarType);
            }
        }
    }
}
