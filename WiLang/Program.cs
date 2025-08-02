using System;
using System.Diagnostics;

namespace WiLang
{
    static class Program
    {
        static void Main()
        {
            Instruction[] bcode = {
                new Instruction(It.PUSH, new Operand(0)),

                new Instruction(It.DUP),
                new Instruction(It.PUSH, new Operand(10000000)),
                new Instruction(It.GT),
                new Instruction(It.JZ, new Operand(6)),
                new Instruction(It.HALT),

                new Instruction(It.INC),
                new Instruction(It.JUMP, new Operand(1)),
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