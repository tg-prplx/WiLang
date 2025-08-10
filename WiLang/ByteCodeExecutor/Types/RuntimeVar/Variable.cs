using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WiLang
{

    public struct Variable
    {
        public Types VarType;
        public object Value;

        public Variable(Types varType, object value)
        {
            VarType = varType;
            Value = value;
        }

        public override string ToString() => VarType switch
        {
            Types.TList => "[" + string.Join(", ", AsList().Select(x => x.ToString())) + "]",
            _ => $"{VarType}: {Value}"
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AsInt() => VarType == Types.TInteger ? (int)Value : throw new InvalidCastException();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double AsFloat() => VarType == Types.TFloat ? (double)Value : throw new InvalidCastException();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string AsString() => VarType == Types.TString ? (string)Value : throw new InvalidCastException();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Variable> AsList() => VarType == Types.TList ? (List<Variable>)Value : throw new InvalidCastException();
        public static Variable FromList(IEnumerable<Variable>? seq = null)
            => new Variable(Types.TList, seq is null ? new List<Variable>() : new List<Variable>(seq));

        public int Length() => VarType == Types.TList ? AsList().Count
                         : VarType == Types.TString ? AsString().Length
                         : throw new InvalidOperationException("Length is defined for list/string");

        public Variable this[int index]
        {
            get
            {
                if (VarType != Types.TList) throw new InvalidOperationException("Indexing requires list");
                return AsList()[index];
            }
            set
            {
                if (VarType != Types.TList) throw new InvalidOperationException("Indexing requires list");
                AsList()[index] = value;
            }
        }

        public bool AsBool() => VarType switch
        {
            Types.TInteger => AsInt() != 0,
            Types.TFloat => AsFloat() != 0.0,
            Types.TString => !string.IsNullOrEmpty(AsString()),
            Types.TList => AsList().Count != 0,
            _ => false
        };

        public Variable Add(Variable b)
        {
            if (VarType == Types.TString || b.VarType == Types.TString)
                return new Variable(Types.TString, this.CastToString().AsString() + b.CastToString().AsString());

            if (VarType == Types.TList && b.VarType == Types.TList)
                return Variable.FromList(AsList().Concat(b.AsList()));

            if (VarType == Types.TList)
            {
                var copy = new List<Variable>(AsList()) { b };
                return new Variable(Types.TList, copy);
            }
            if (b.VarType == Types.TList)
            {
                var copy = new List<Variable>(b.AsList().Count + 1);
                copy.Add(this);
                copy.AddRange(b.AsList());
                return new Variable(Types.TList, copy);
            }

            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() + b.AsFloat());
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() + b.AsInt());

            throw new InvalidOperationException("Add: unsupported type combination");
        }

        public Variable Sub(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() - b.AsFloat());
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() - b.AsInt());
            throw new InvalidOperationException("Sub: unsupported type combination");
        }

        public Variable Mul(Variable b)
        {
            if (VarType == Types.TList && b.VarType == Types.TInteger)
            {
                int n = Math.Max(0, b.AsInt());
                var src = AsList();
                var res = new List<Variable>(src.Count * n);
                for (int i = 0; i < n; i++) res.AddRange(src);
                return new Variable(Types.TList, res);
            }

            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() * b.AsFloat());
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() * b.AsInt());
            throw new InvalidOperationException("Mul: unsupported type combination");
        }

        public Variable Div(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() / b.AsFloat());
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() / b.AsInt());
            throw new InvalidOperationException("Div: unsupported type combination");
        }

        public Variable Mod(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return new Variable(Types.TFloat, this.AsFloat() % b.AsFloat());
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return new Variable(Types.TInteger, this.AsInt() % b.AsInt());
            throw new InvalidOperationException("Mod: unsupported type combination");
        }

        public bool EqualsTo(Variable b)
        {
            if (VarType == Types.TList && b.VarType == Types.TList)
            {
                var aL = AsList(); var bL = b.AsList();
                if (aL.Count != bL.Count) return false;
                for (int i = 0; i < aL.Count; i++)
                    if (!aL[i].EqualsTo(bL[i])) return false;
                return true;
            }
            if (VarType == Types.TString || b.VarType == Types.TString)
                return this.CastToString().AsString() == b.CastToString().AsString();
            if (VarType == Types.TFloat || b.VarType == Types.TFloat)
                return this.AsFloat() == b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger)
                return this.AsInt() == b.AsInt();
            throw new InvalidOperationException("Eq: unsupported type combination");
        }

        public bool NotEqualsTo(Variable b) => !EqualsTo(b);

        public bool LessThan(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat) return this.AsFloat() < b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger) return this.AsInt() < b.AsInt();
            throw new InvalidOperationException("LT: unsupported type combination");
        }
        public bool GreaterThan(Variable b)
        {
            if (VarType == Types.TFloat || b.VarType == Types.TFloat) return this.AsFloat() > b.AsFloat();
            if (VarType == Types.TInteger && b.VarType == Types.TInteger) return this.AsInt() > b.AsInt();
            throw new InvalidOperationException("GT: unsupported type combination");
        }
        public bool LessOrEqual(Variable b) => !GreaterThan(b);
        public bool GreaterOrEqual(Variable b) => !LessThan(b);

        public Variable CastToInt() => VarType switch
        {
            Types.TInteger => this,
            Types.TFloat => new Variable(Types.TInteger, (int)AsFloat()),
            Types.TString => int.TryParse(AsString(), out var iv) ? new Variable(Types.TInteger, iv)
                                                                   : throw new InvalidCastException($"Cannot cast '{Value}' to int."),
            _ => throw new InvalidCastException($"Cannot cast {VarType} to int.")
        };

        public Variable CastToFloat() => VarType switch
        {
            Types.TFloat => this,
            Types.TInteger => new Variable(Types.TFloat, (double)AsInt()),
            Types.TString => double.TryParse(AsString(), out var fv) ? new Variable(Types.TFloat, fv)
                                                                      : throw new InvalidCastException($"Cannot cast '{Value}' to float."),
            _ => throw new InvalidCastException($"Cannot cast {VarType} to float.")
        };

        public Variable CastToString() => VarType switch
        {
            Types.TList => new Variable(Types.TString, "[" + string.Join(", ", AsList().Select(v => v.CastToString().AsString())) + "]"),
            _ => new Variable(Types.TString, Value?.ToString() ?? "None")
        };

        public Variable Clone()
        {
            if (VarType == Types.TList)
                return new Variable(Types.TList, AsList().Select(v => v.Clone()).ToList());
            return new Variable(VarType, Value);
        }

        public static Variable operator +(Variable a, Variable b) => a.Add(b);
        public static Variable operator -(Variable a, Variable b) => a.Sub(b);
        public static Variable operator *(Variable a, Variable b) => a.Mul(b);
        public static Variable operator /(Variable a, Variable b) => a.Div(b);
        public static Variable operator %(Variable a, Variable b) => a.Mod(b);

        public static bool operator ==(Variable a, Variable b) => a.EqualsTo(b);
        public static bool operator !=(Variable a, Variable b) => !a.EqualsTo(b);
        public static bool operator <(Variable a, Variable b) => a.LessThan(b);
        public static bool operator >(Variable a, Variable b) => a.GreaterThan(b);
        public static bool operator <=(Variable a, Variable b) => a.LessOrEqual(b);
        public static bool operator >=(Variable a, Variable b) => a.GreaterOrEqual(b);

        public override bool Equals(object? obj) => obj is Variable v && this == v;
        public override int GetHashCode() => (VarType, Value).GetHashCode();
    }
}
