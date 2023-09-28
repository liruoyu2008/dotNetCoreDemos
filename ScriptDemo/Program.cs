using System;

namespace ScriptDemo
{
    class Program
    {
        public int A = 1;
        public int B = 2;

        static void Main(string[] args)
        {
            var x1 = new ScriptTest2();
            x1.Test();
            var x2 = new ScriptTest2();
            x2.Test();

            var resA1 = x1.FuncA();
            var resA2 = x2.FuncA();

            var resB1 = x1.FuncB();
            var resB2 = x2.FuncB();

            Console.ReadLine();
        }
    }
}
