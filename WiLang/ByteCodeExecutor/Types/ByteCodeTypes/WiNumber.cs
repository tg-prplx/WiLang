using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public struct WiNumber
    {
        public int? Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WiNumber(int? value) { Value = value; }
    }
}
