using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    struct WiStack<T>
    {
        private T[] data;
        private int sp;

        public WiStack(int size)
        {
            data = new T[size];
            sp = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T value)
        {
            data[sp++] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Pop()
        {
            return data[--sp];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek()
        {
            return data[sp - 1];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dup()
        {
            if (sp > 0)
            {
                data[sp] = data[sp - 1];
                sp++;
            }
            else throw new InvalidOperationException();
        }

        public int Count => sp;
    }

}
