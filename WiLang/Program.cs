using System;
using System.Diagnostics;

namespace WiLang
{
    static class Program
    {
        static void Main()
        {
            Instruction[] bcode = {
                new Instruction(It.PUSH, 10_000_000),
                new Instruction(It.DUP),          
                new Instruction(It.JZ, 5),
                new Instruction(It.DEC),
                new Instruction(It.JUMP, 1),
                new Instruction(It.HALT),
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