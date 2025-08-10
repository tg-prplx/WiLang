using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public class CompDelegates
    {
        public static readonly Func<Variable, Variable, bool> lt = static (a, b) => a < b;
        public static readonly Func<Variable, Variable, bool> gt = static (a, b) => a > b;
        public static readonly Func<Variable, Variable, bool> le = static (a, b) => a <= b;
        public static readonly Func<Variable, Variable, bool> ge = static (a, b) => a >= b;
        public static readonly Func<Variable, Variable, bool> eq = static (a, b) => a == b;
        public static readonly Func<Variable, Variable, bool> ne = static (a, b) => a != b;
    }
}
