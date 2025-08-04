using System;
using System.Diagnostics;

namespace WiLang
{
    static class Program
    {
        static void Main()
        {
            Instruction[] bcode = {
            // i = 1
            new Instruction(It.PUSH, 1),
            new Instruction(It.STORE, "i"),

            // sum = 0
            new Instruction(It.PUSH, 0),
            new Instruction(It.STORE, "sum"),

            // --- loop_start: ---
            // if i > 10 goto end
            new Instruction(It.LOAD, "i"),         // stack: i
            new Instruction(It.PUSH, 10),          // stack: i, 10
            new Instruction(It.GT),                // stack: i > 10 ? 1 : 0
            new Instruction(It.JZ, 10),            // если 0 (не больше), то идём дальше, если 1 — прыгаем к end (10-й инструкции, отсчёт с 0)

            // sum = sum + i
            new Instruction(It.LOAD, "sum"),
            new Instruction(It.LOAD, "i"),
            new Instruction(It.ADD),
            new Instruction(It.STORE, "sum"),

            // i = i + 1
            new Instruction(It.LOAD, "i"),
            new Instruction(It.PUSH, 1),
            new Instruction(It.ADD),
            new Instruction(It.STORE, "i"),

            // jump to loop_start
            new Instruction(It.JUMP, 3),

            // --- end: ---
            new Instruction(It.LOAD, "sum"),
            new Instruction(It.PRINT),
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