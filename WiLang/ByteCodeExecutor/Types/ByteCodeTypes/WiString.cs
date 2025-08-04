using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public struct WiString
    {
        public string? Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WiString(string? value) { Value = value; }
    }
}
