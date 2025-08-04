using System;
using System.Collections.Generic;
using System.Linq;
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

        public override string ToString()
        {
            return $"{VarType}: {Value}";
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AsInt() => VarType == Types.TInteger ? (int)Value : throw new InvalidCastException();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string AsString() => VarType == Types.TString ? (string)Value : throw new InvalidCastException();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double AsFloat() => VarType == Types.TFloat ? (double)Value : throw new InvalidCastException();
        
        public Variable CastToInt()
        {
            switch (VarType)
            {
                case Types.TInteger:
                    return this;
                case Types.TFloat:
                    return new Variable(Types.TInteger, (int)(double)Value);
                case Types.TString:
                    if (int.TryParse((string)Value, out var iv))
                        return new Variable(Types.TInteger, iv);
                    throw new InvalidCastException($"Cannot cast string '{Value}' to int.");
                default:
                    throw new InvalidCastException($"Cannot cast {VarType} to int.");
            }
        }

        public Variable CastToFloat()
        {
            switch (VarType)
            {
                case Types.TFloat:
                    return this;
                case Types.TInteger:
                    return new Variable(Types.TFloat, Convert.ToDouble(Value));
                case Types.TString:
                    if (double.TryParse((string)Value, out var fv))
                        return new Variable(Types.TFloat, fv);
                    throw new InvalidCastException($"Cannot cast string '{Value}' to float.");
                default:
                    throw new InvalidCastException($"Cannot cast {VarType} to float.");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable CastToString()
        {
            return new Variable(Types.TString, Value.ToString() ?? "None");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variable Clone() => new Variable(this.VarType, this.Value);
    }
}