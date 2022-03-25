using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace SpectreConsoleDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var c = new CanvasImage("1.jpg");
            AnsiConsole.Write(c);
            Console.ReadKey();
        }

    }
}
