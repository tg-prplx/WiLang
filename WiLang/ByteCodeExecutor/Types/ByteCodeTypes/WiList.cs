using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public readonly struct WiList<T>
    {
        public List<T> Value { get; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WiList(List<T>? value)
        {
            Value = value ?? new List<T>();
        }
    }
}
