using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public struct Operand
    {
        public WiValue? Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(WiValue value) { Value = value; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(WiNumber num) { Value = new WiNumberValue(num); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(WiFloat flt) { Value = new WiFloatValue(flt); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Operand(WiString str) { Value = new WiStringValue(str); }
    }
}
