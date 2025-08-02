using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    static class Ariphmetics
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public Variable AddOp(Variable a, Variable b)
        {
            if (a.VarType == Types.TInteger)
                return new Variable(Types.TInteger, a.AsInt() + b.AsInt());
            if (a.VarType == Types.TFloat)
                return new Variable(Types.TFloat, a.AsFloat() + b.AsFloat());
            if (a.VarType == Types.TString)
                return new Variable(Types.TString, a.AsString() + b.AsString());
            throw new Exception("Add: unsupported type");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public Variable SubOp(Variable a, Variable b)
        {
            if (a.VarType == Types.TInteger)
                return new Variable(Types.TInteger, a.AsInt() - b.AsInt());
            if (a.VarType == Types.TFloat)
                return new Variable(Types.TFloat, a.AsFloat() - b.AsFloat());
            throw new Exception("Sub: unsupported type");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public Variable MulOp(Variable a, Variable b)
        {
            if (a.VarType == Types.TInteger)
                return new Variable(Types.TInteger, a.AsInt() * b.AsInt());
            if (a.VarType == Types.TFloat)
                return new Variable(Types.TFloat, a.AsFloat() * b.AsFloat());
            throw new Exception("Mul: unsupported type");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public Variable DivOp(Variable a, Variable b)
        {
            if (a.VarType == Types.TInteger)
                return new Variable(Types.TInteger, a.AsInt() / b.AsInt());
            if (a.VarType == Types.TFloat)
                return new Variable(Types.TFloat, a.AsFloat() / b.AsFloat());
            throw new Exception("Div: unsupported type");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public Variable ModOp(Variable a, Variable b)
        {
            if (a.VarType == Types.TInteger)
                return new Variable(Types.TInteger, a.AsInt() % b.AsInt());
            if (a.VarType == Types.TFloat)
                return new Variable(Types.TFloat, a.AsFloat() % b.AsFloat());
            throw new Exception("Mod: unsupported type");
        }
    }
}
