using System;
using System.Runtime.CompilerServices;

namespace WiLang
{
    struct Variable
    {
        public Types VarType;
        public object Value;

        public Variable(Types varType, object value)
        {
            VarType = varType;
            Value = value;
        }

        public override string ToString() => $"{VarType}: {Value}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AsInt() => VarType == Types.TInteger ? (int)Value : throw new InvalidCastException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double AsFloat() => VarType == Types.TFloat ? (double)Value : throw new InvalidCastException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string AsString() => VarType == Types.TString ? (string)Value : throw new InvalidCastException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable Add(Variable b)
        {
            if (VarType == Types.TString || b.VarType == Types.TString)
                return new Variable(Types.TString, this.AsString() + b.AsString());

            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() + b.AsFloat());

            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() + b.AsInt());

            throw new InvalidOperationException("Add: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AsBool()
        {
            return VarType switch
            {
                Types.TInteger => AsInt() != 0,
                Types.TFloat => AsFloat() != 0.0,
                Types.TString => !string.IsNullOrEmpty(AsString()),
                _ => false
            };
        }

        public Variable Sub(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() - b.AsFloat());

            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() - b.AsInt());

            throw new InvalidOperationException("Sub: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable Mul(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() * b.AsFloat());

            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() * b.AsInt());

            throw new InvalidOperationException("Mul: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable Div(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() / b.AsFloat());

            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() / b.AsInt());

            throw new InvalidOperationException("Div: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable Mod(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() % b.AsFloat());

            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() % b.AsInt());

            throw new InvalidOperationException("Mod: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualsTo(Variable b)
        {
            if (VarType == Types.TString || b.VarType == Types.TString)
                return this.AsString() == b.AsString();
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return this.AsFloat() == b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return this.AsInt() == b.AsInt();
            throw new InvalidOperationException("Eq: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEqualsTo(Variable b) => !EqualsTo(b);

        public bool LessThan(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return this.AsFloat() < b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return this.AsInt() < b.AsInt();
            throw new InvalidOperationException("LT: unsupported type combination");
        }

        public bool GreaterThan(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return this.AsFloat() > b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return this.AsInt() > b.AsInt();
            throw new InvalidOperationException("GT: unsupported type combination");
        }

        public bool LessOrEqual(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return this.AsFloat() <= b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return this.AsInt() <= b.AsInt();
            throw new InvalidOperationException("LE: unsupported type combination");
        }

        public bool GreaterOrEqual(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return this.AsFloat() >= b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return this.AsInt() >= b.AsInt();
            throw new InvalidOperationException("GE: unsupported type combination");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable CastToInt()
        {
            switch (VarType)
            {
                case Types.TInteger:
                    return this;
                case Types.TFloat:
                    return new Variable(Types.TInteger, (int)AsFloat());
                case Types.TString:
                    if (int.TryParse(AsString(), out var iv))
                        return new Variable(Types.TInteger, iv);
                    throw new InvalidCastException($"Cannot cast string '{Value}' to int.");
                default:
                    throw new InvalidCastException($"Cannot cast {VarType} to int.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable CastToFloat()
        {
            switch (VarType)
            {
                case Types.TFloat:
                    return this;
                case Types.TInteger:
                    return new Variable(Types.TFloat, (double)AsInt());
                case Types.TString:
                    if (double.TryParse(AsString(), out var fv))
                        return new Variable(Types.TFloat, fv);
                    throw new InvalidCastException($"Cannot cast string '{Value}' to float.");
                default:
                    throw new InvalidCastException($"Cannot cast {VarType} to float.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable CastToString() =>
            new Variable(Types.TString, Value.ToString() ?? "None");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable Clone() => new Variable(this.VarType, this.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable operator +(Variable a, Variable b) => a.Add(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable operator -(Variable a, Variable b) => a.Sub(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable operator *(Variable a, Variable b) => a.Mul(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable operator /(Variable a, Variable b) => a.Div(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable operator %(Variable a, Variable b) => a.Mod(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Variable a, Variable b) => a.EqualsTo(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Variable a, Variable b) => !a.EqualsTo(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Variable a, Variable b) => a.LessThan(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Variable a, Variable b) => a.GreaterThan(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Variable a, Variable b) => a.LessOrEqual(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Variable a, Variable b) => a.GreaterOrEqual(b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public override bool Equals(object obj)
        {
            if (obj is Variable v)
                return this == v;
            return false;
        }
        public override int GetHashCode() => (VarType, Value).GetHashCode();
    }
}
