using System.Runtime.CompilerServices;

namespace WiLang
{
    public abstract record WiValue;

    public record WiNumberValue(WiNumber Value) : WiValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WiNumberValue(int v) => new WiNumberValue(new WiNumber(v));
    }

    public record WiFloatValue(WiFloat Value) : WiValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WiFloatValue(double v) => new WiFloatValue(new WiFloat((float)v));
    }

    public record WiStringValue(WiString Value) : WiValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WiStringValue(string v) => new WiStringValue(new WiString(v));
    }
}
