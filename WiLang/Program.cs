using System;
using System.Diagnostics;

namespace WiLang
{
    static class Program
    {
        static void Main()
        {
            Instruction[] bcode = {
                new Instruction(It.PUSH, 1),
                new Instruction(It.DUP),
                new Instruction(It.PUSH, 10000000),
                new Instruction(It.GT),
                new Instruction(It.JZ, 6),
                new Instruction(It.HALT),
                new Instruction(It.INC),
                new Instruction(It.JUMP, 1),
            };

            var vm = new VM();
            var sw = new Stopwatch();
            sw.Start();
            vm.Execv(bcode);
            sw.Stop();
            Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
        }
    }
}