using System;
using System.Diagnostics;

namespace WiLang
{
    static class Program
    {
        static void Main()
        {
            object[] code = {
            It.PUSH, 0,         // 0
            It.STORE, "i",      // 2

            It.LOAD, "i",       // 4
            It.PUSH, 10000000,     // 6
            It.GT,              // 8
            It.JZ, 13,          // 9
            It.JUMP, 24,        // 11

            It.LOAD, "i",       // 13
            // It.PRINT,        // 15
            It.LOAD, "i",       // 15
            It.PUSH, 1,         // 17
            It.ADD,             // 19
            It.STORE, "i",      // 20
            It.JUMP, 4,         // 22

            It.HALT             // 23
        };
            var vm = new VM();
            var sw = new Stopwatch();
            sw.Start();
            vm.Execv(code);
            sw.Stop();
            Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
        }
    }
}