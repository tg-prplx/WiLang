using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public abstract record WiValue;
    record WiNumberValue(WiNumber Value) : WiValue;
    record WiFloatValue(WiFloat Value) : WiValue;
    record WiStringValue(WiString Value) : WiValue;
}
