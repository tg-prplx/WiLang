using System.Runtime.CompilerServices;

namespace WiLang
{
    public abstract record WiValue;

    public record WiNumberValue(WiNumber Value) : WiValue
    {
        public static implicit operator WiNumberValue(int v) => new WiNumberValue(new WiNumber(v));
    }

    public record WiFloatValue(WiFloat Value) : WiValue
    {
        public static implicit operator WiFloatValue(double v) => new WiFloatValue(new WiFloat((float)v));
    }

    public record WiStringValue(WiString Value) : WiValue
    {
        public static implicit operator WiStringValue(string v) => new WiStringValue(new WiString(v));
    }
    public record WiListValue<T>(WiList<T> Value) : WiValue
    {
        public static implicit operator WiListValue<T>(List<T> list) => new(new WiList<T>(list));
        public static implicit operator WiListValue<T>(T[] array) => new(new WiList<T>(array.ToList()));
        public static implicit operator WiListValue<T>(WiList<T> list) => new(list);
    }
}
