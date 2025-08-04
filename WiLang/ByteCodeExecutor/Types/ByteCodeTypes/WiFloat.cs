using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public struct WiFloat
    {
        public float? Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WiFloat(float? value) { Value = value; }
    }
}
